using WordCounter.Interfaces;
using WordCounter.Services;
using NSubstitute;

namespace WordCounter.Tests;

public class WordCounterServiceTests
{
    private readonly IFileReader _mockFileReader;
    private readonly IWordParser _mockWordParser;
    private readonly IWordCounter _mockWordCounter;
    private readonly IWordAggregator _mockWordAggregator;
    private readonly WordCounterService _service;

    public WordCounterServiceTests()
    {
        _mockFileReader = Substitute.For<IFileReader>();
        _mockWordParser = Substitute.For<IWordParser>();
        _mockWordCounter = Substitute.For<IWordCounter>();
        _mockWordAggregator = Substitute.For<IWordAggregator>();
        _service = new WordCounterService(_mockFileReader, _mockWordParser, _mockWordCounter, _mockWordAggregator);
    }

    [Fact]
    public async Task CountWordsInFilesAsync_SingleFile_ProcessesPipelineCorrectly()
    {
        // Arrange
        var filePaths = new[] { "test.txt" };
        var expectedResult = new Dictionary<string, int> { { "hello", 2 }, { "world", 1 } };
        
        _mockFileReader.ReadCharactersAsync("test.txt")
                      .Returns(CreateAsyncCharEnumerable("hello world hello"));
                      
        _mockWordParser.ParseWordsAsync(Arg.Any<IAsyncEnumerable<char>>())
                      .Returns(CreateAsyncWordEnumerable(new[] { "hello", "world", "hello" }));

        _mockWordCounter.CountWordsAsync(Arg.Any<IAsyncEnumerable<string>>())
                       .Returns(Task.FromResult(new Dictionary<string, int> { { "hello", 2 }, { "world", 1 } }));

        _mockWordAggregator.AggregateAsync(Arg.Any<IEnumerable<Task<Dictionary<string, int>>>>())
                          .Returns(Task.FromResult(expectedResult));

        // Act
        var result = await _service.CountWordsInFilesAsync(filePaths);

        // Assert
        Assert.Equal(expectedResult, result);
        
        // Verify calls were made (simplified verification)
        await _mockWordAggregator.Received(1).AggregateAsync(Arg.Any<IEnumerable<Task<Dictionary<string, int>>>>());
    }

    [Fact]
    public async Task CountWordsInFilesAsync_EmptyFileList_ReturnsEmptyDictionary()
    {
        // Arrange
        var filePaths = new string[0];

        // Act
        var result = await _service.CountWordsInFilesAsync(filePaths);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task CountWordsInFilesAsync_NullFilePaths_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CountWordsInFilesAsync(null!));
    }

    private static async IAsyncEnumerable<char> CreateAsyncCharEnumerable(string input)
    {
        foreach (char c in input)
        {
            await Task.Yield();
            yield return c;
        }
    }

    private static async IAsyncEnumerable<string> CreateAsyncWordEnumerable(IEnumerable<string> items)
    {
        foreach (var item in items)
        {
            await Task.Yield();
            yield return item;
        }
    }
}
