using WordCounter.Interfaces;
using NSubstitute;
using WordCounterService = WordCounter.Services.WordCounter;

namespace WordCounter.Tests;

public class WordCounterTests
{
    private readonly WordCounterService _wordCounter;

    public WordCounterTests()
    {
        _wordCounter = new WordCounterService();
    }

    [Fact]
    public async Task CountWordsAsync_SingleWords_ReturnsWordCounts()
    {
        // Arrange
        var words = CreateAsyncEnumerable(new[] { "hello", "world", "hello" });

        // Act
        var result = await _wordCounter.CountWordsAsync(words);

        // Assert
        Assert.Equal(2, result["hello"]);
        Assert.Equal(1, result["world"]);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task CountWordsAsync_DifferentWords_CountsEachOnce()
    {
        // Arrange
        var words = CreateAsyncEnumerable(new[] { "apple", "banana", "cherry" });

        // Act
        var result = await _wordCounter.CountWordsAsync(words);

        // Assert
        Assert.Equal(1, result["apple"]);
        Assert.Equal(1, result["banana"]);
        Assert.Equal(1, result["cherry"]);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task CountWordsAsync_EmptyWordStream_ReturnsEmptyDictionary()
    {
        // Arrange
        var words = CreateAsyncEnumerable(Array.Empty<string>());

        // Act
        var result = await _wordCounter.CountWordsAsync(words);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task CountWordsAsync_WithWhitespaceWords_IgnoresInvalidWords()
    {
        // Arrange
        var words = CreateAsyncEnumerable(new[] { "hello", "", "world", "   ", "test" });

        // Act
        var result = await _wordCounter.CountWordsAsync(words);

        // Assert
        Assert.Equal(1, result["hello"]);
        Assert.Equal(1, result["world"]);
        Assert.Equal(1, result["test"]);
        Assert.Equal(3, result.Count);
        Assert.False(result.ContainsKey(""));
        Assert.False(result.ContainsKey("   "));
    }

    [Fact]
    public async Task CountWordsAsync_LargeNumberOfWords_HandlesEfficiently()
    {
        // Arrange
        var manyWords = Enumerable.Repeat("test", 10000).ToArray();
        var words = CreateAsyncEnumerable(manyWords);

        // Act
        var result = await _wordCounter.CountWordsAsync(words);

        // Assert
        Assert.Equal(10000, result["test"]);
        Assert.Single(result);
    }

    [Fact]
    public async Task CountWordsAsync_MixedFrequencies_CountsCorrectly()
    {
        // Arrange
        var words = CreateAsyncEnumerable(new[] 
        { 
            "apple", "banana", "apple", "cherry", "apple", 
            "banana", "date", "elderberry", "apple" 
        });

        // Act
        var result = await _wordCounter.CountWordsAsync(words);

        // Assert
        Assert.Equal(4, result["apple"]);     // Most frequent
        Assert.Equal(2, result["banana"]);    // Second most frequent
        Assert.Equal(1, result["cherry"]);    // Once
        Assert.Equal(1, result["date"]);      // Once
        Assert.Equal(1, result["elderberry"]); // Once
        Assert.Equal(5, result.Count);
    }

    private static async IAsyncEnumerable<string> CreateAsyncEnumerable(IEnumerable<string> items)
    {
        foreach (var item in items)
        {
            yield return item;
        }
    }
}
