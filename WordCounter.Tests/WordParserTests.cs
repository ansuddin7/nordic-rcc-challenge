using WordCounter.Services;

namespace WordCounter.Tests;

public class WordParserTests
{
    private readonly WordParser _parser = new();

    [Fact]
    public async Task ParseWordsAsync_SimpleText_ReturnsCleanWords()
    {
        var characters = "Hello World".ToAsyncEnumerable();

        var actualWords = new List<string>();
        await foreach (var word in _parser.ParseWordsAsync(characters))
            actualWords.Add(word);

        Assert.Equal(new[] { "hello", "world" }, actualWords);
    }

    [Fact]
    public async Task ParseWordsAsync_TextWithPunctuation_RemovesPunctuationAndReturnsWords()
    {
        var characters = "Hello, world! How are you?".ToAsyncEnumerable();

        var actualWords = new List<string>();
        await foreach (var word in _parser.ParseWordsAsync(characters))
            actualWords.Add(word);

        Assert.Equal(new[] { "hello", "world", "how", "are", "you" }, actualWords);
    }

    [Fact]
    public async Task ParseWordsAsync_EmptyInput_ReturnsNoWords()
    {
        var characters = "".ToAsyncEnumerable();

        var actualWords = new List<string>();
        await foreach (var word in _parser.ParseWordsAsync(characters))
            actualWords.Add(word);

        Assert.Empty(actualWords);
    }

    [Fact]
    public async Task ParseWordsAsync_OnlyWhitespace_ReturnsNoWords()
    {
        var characters = "   \t\n  \r\n  ".ToAsyncEnumerable();

        var actualWords = new List<string>();
        await foreach (var word in _parser.ParseWordsAsync(characters))
            actualWords.Add(word);

        Assert.Empty(actualWords);
    }

    [Fact]
    public async Task ParseWordsAsync_OnlyPunctuation_ReturnsNoWords()
    {
        var characters = "!@#$%^&*()_+-=[]{}|;':\",./<>?".ToAsyncEnumerable();

        var actualWords = new List<string>();
        await foreach (var word in _parser.ParseWordsAsync(characters))
            actualWords.Add(word);

        Assert.Empty(actualWords);
    }

    [Fact]
    public async Task ParseWordsAsync_MixedCase_ReturnsLowercaseWords()
    {
        var characters = "HELLO world TeSt".ToAsyncEnumerable();

        var actualWords = new List<string>();
        await foreach (var word in _parser.ParseWordsAsync(characters))
            actualWords.Add(word);

        Assert.Equal(new[] { "hello", "world", "test" }, actualWords);
    }

    [Fact]
    public async Task ParseWordsAsync_NumbersAndLetters_ReturnsWordsWithNumbers()
    {
        var characters = "test123 456word hello2world".ToAsyncEnumerable();

        var actualWords = new List<string>();
        await foreach (var word in _parser.ParseWordsAsync(characters))
            actualWords.Add(word);

        Assert.Equal(new[] { "test123", "456word", "hello2world" }, actualWords);
    }

    [Fact]
    public async Task ParseWordsAsync_ComplexPunctuation_HandlesCorrectly()
    {
        var characters = "word1 [word2] (word3) {word4} 'word5' \"word6\"".ToAsyncEnumerable();

        var actualWords = new List<string>();
        await foreach (var word in _parser.ParseWordsAsync(characters))
            actualWords.Add(word);

        Assert.Equal(new[] { "word1", "word2", "word3", "word4", "word5", "word6" }, actualWords);
    }

    [Fact]
    public async Task ParseWordsAsync_MultilineText_HandlesNewlinesCorrectly()
    {
        var characters = "word1\nword2\r\nword3\tword4".ToAsyncEnumerable();

        var actualWords = new List<string>();
        await foreach (var word in _parser.ParseWordsAsync(characters))
            actualWords.Add(word);

        Assert.Equal(new[] { "word1", "word2", "word3", "word4" }, actualWords);
    }

    [Fact]
    public async Task ParseWordsAsync_WordsWithMultipleSpaces_HandlesCorrectly()
    {
        var characters = "word1    word2\t\t\tword3".ToAsyncEnumerable();

        var actualWords = new List<string>();
        await foreach (var word in _parser.ParseWordsAsync(characters))
            actualWords.Add(word);

        Assert.Equal(new[] { "word1", "word2", "word3" }, actualWords);
    }

    [Fact]
    public async Task ParseWordsAsync_EndsWithoutWhitespace_HandlesFinalWord()
    {
        var characters = "hello world test".ToAsyncEnumerable();

        var actualWords = new List<string>();
        await foreach (var word in _parser.ParseWordsAsync(characters))
            actualWords.Add(word);

        Assert.Equal(new[] { "hello", "world", "test" }, actualWords);
    }
}

// Helper extension method for testing
public static class StringExtensions
{
    public static async IAsyncEnumerable<char> ToAsyncEnumerable(this string input)
    {
        foreach (char c in input)
        {
            await Task.Yield(); // Make it actually async for realistic testing
            yield return c;
        }
    }
}
