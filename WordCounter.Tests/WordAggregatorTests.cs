using WordCounter.Core.Services;

namespace WordCounter.Tests;

public class WordAggregatorTests
{
    private readonly WordAggregator _aggregator = new();

    [Fact]
    public async Task AggregateAsync_SingleDictionary_ReturnsSameDictionary()
    {
        // Arrange
        var dict = new Dictionary<string, int> { { "hello", 2 }, { "world", 1 } };
        var tasks = new[] { Task.FromResult(dict) };

        // Act
        var result = await _aggregator.AggregateAsync(tasks);

        // Assert
        Assert.Equal(2, result["hello"]);
        Assert.Equal(1, result["world"]);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task AggregateAsync_MultipleDictionaries_AggregatesCounts()
    {
        // Arrange
        var dict1 = new Dictionary<string, int> { { "hello", 2 }, { "world", 1 } };
        var dict2 = new Dictionary<string, int> { { "hello", 1 }, { "test", 3 } };
        var tasks = new[] { Task.FromResult(dict1), Task.FromResult(dict2) };

        // Act
        var result = await _aggregator.AggregateAsync(tasks);

        // Assert
        Assert.Equal(3, result["hello"]);  // 2 + 1
        Assert.Equal(1, result["world"]);  // 1
        Assert.Equal(3, result["test"]);   // 3
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task AggregateAsync_EmptyDictionaries_ReturnsEmpty()
    {
        // Arrange
        var tasks = new[] { Task.FromResult(new Dictionary<string, int>()) };

        // Act
        var result = await _aggregator.AggregateAsync(tasks);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task AggregateAsync_NoTasks_ReturnsEmpty()
    {
        // Arrange
        var tasks = Array.Empty<Task<Dictionary<string, int>>>();

        // Act
        var result = await _aggregator.AggregateAsync(tasks);

        // Assert
        Assert.Empty(result);
    }
}
