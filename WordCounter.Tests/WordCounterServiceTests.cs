using WordCounter.Core.Interfaces;
using WordCounter.Core.Services;
using NSubstitute;

namespace WordCounter.Tests;

public class WordCounterServiceTests
{
    private readonly IFileReader _mockFileReader;
    private readonly IWordCounter _mockWordCounter;
    private readonly IWordAggregator _mockWordAggregator;
    private readonly WordCounterService _service;

    public WordCounterServiceTests()
    {
        _mockFileReader = Substitute.For<IFileReader>();
        _mockWordCounter = Substitute.For<IWordCounter>();
        _mockWordAggregator = Substitute.For<IWordAggregator>();
        _service = new WordCounterService(_mockFileReader, _mockWordCounter, _mockWordAggregator);
    }

    [Fact]
    public async Task CountWordsInFilesAsync_SingleFile_ReturnsAggregatedResult()
    {
        // Arrange
        var filePaths = new[] { "test.txt" };
        var expectedResult = new Dictionary<string, int> { { "hello", 2 } };
        
        _mockWordAggregator.AggregateAsync(Arg.Any<IEnumerable<Task<Dictionary<string, int>>>>())
                          .Returns(expectedResult);

        // Act
        var result = await _service.CountWordsInFilesAsync(filePaths);

        // Assert
        Assert.Equal(expectedResult, result);
        await _mockWordAggregator.Received(1).AggregateAsync(Arg.Any<IEnumerable<Task<Dictionary<string, int>>>>());
    }

    [Fact]
    public async Task CountWordsInFilesAsync_MultipleFiles_ProcessesAllFiles()
    {
        // Arrange
        var filePaths = new[] { "file1.txt", "file2.txt" };
        var expectedResult = new Dictionary<string, int> { { "hello", 3 } };
        
        _mockWordAggregator.AggregateAsync(Arg.Any<IEnumerable<Task<Dictionary<string, int>>>>())
                          .Returns(expectedResult);

        // Act
        var result = await _service.CountWordsInFilesAsync(filePaths);

        // Assert
        Assert.Equal(expectedResult, result);
        await _mockWordAggregator.Received(1).AggregateAsync(Arg.Is<IEnumerable<Task<Dictionary<string, int>>>>(tasks => tasks.Count() == 2));
    }

    [Fact]
    public async Task CountWordsInFilesAsync_EmptyFilePaths_ReturnsEmptyDictionary()
    {
        // Arrange
        var filePaths = Array.Empty<string>();

        // Act
        var result = await _service.CountWordsInFilesAsync(filePaths);

        // Assert
        Assert.Empty(result);
        await _mockWordAggregator.DidNotReceive().AggregateAsync(Arg.Any<IEnumerable<Task<Dictionary<string, int>>>>());
    }

    [Fact]
    public async Task CountWordsInFilesAsync_NullFilePaths_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _service.CountWordsInFilesAsync(null!));
    }
}
