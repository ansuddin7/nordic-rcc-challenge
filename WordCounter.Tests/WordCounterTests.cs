using WordCounter.Interfaces;
using NSubstitute;
using WordCounterService = WordCounter.Services.WordCounter;

namespace WordCounter.Tests;

public class WordCounterTests
{
    private readonly IWordParser _mockParser;
    private readonly WordCounterService _wordCounter;

    public WordCounterTests()
    {
        _mockParser = Substitute.For<IWordParser>();
        _wordCounter = new WordCounterService(_mockParser);
    }

    [Fact]
    public async Task CountWordsAsync_SingleLine_ReturnsWordCounts()
    {
        // Arrange
        var lines = CreateAsyncEnumerable(new[] { "hello world hello" });
        _mockParser.ParseWords("hello world hello")
                  .Returns(new[] { "hello", "world", "hello" });

        // Act
        var result = await _wordCounter.CountWordsAsync(lines);

        // Assert
        Assert.Equal(2, result["hello"]);
        Assert.Equal(1, result["world"]);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task CountWordsAsync_MultipleLines_AggregatesCounts()
    {
        // Arrange
        var lines = CreateAsyncEnumerable(new[] { "hello world", "world test" });
        _mockParser.ParseWords("hello world")
                  .Returns(new[] { "hello", "world" });
        _mockParser.ParseWords("world test")
                  .Returns(new[] { "world", "test" });

        // Act
        var result = await _wordCounter.CountWordsAsync(lines);

        // Assert
        Assert.Equal(1, result["hello"]);
        Assert.Equal(2, result["world"]);
        Assert.Equal(1, result["test"]);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task CountWordsAsync_EmptyLines_ReturnsEmptyDictionary()
    {
        // Arrange
        var lines = CreateAsyncEnumerable(Array.Empty<string>());

        // Act
        var result = await _wordCounter.CountWordsAsync(lines);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void CountWordsAsync_NullParser_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            new WordCounterService(null!));
    }

    private static async IAsyncEnumerable<string> CreateAsyncEnumerable(IEnumerable<string> items)
    {
        foreach (var item in items)
        {
            yield return item;
        }
    }
}
