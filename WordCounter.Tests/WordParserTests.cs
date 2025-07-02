using WordCounter.Services;

namespace WordCounter.Tests;

public class WordParserTests
{
    private readonly WordParser _parser = new();

    [Fact]
    public void ParseWords_SimpleText_ReturnsWords()
    {
        var result = _parser.ParseWords("Go and do that thing").ToList();
        
        Assert.Equal(new[] { "go", "and", "do", "that", "thing" }, result);
    }

    [Fact]
    public void ParseWords_TextWithPunctuation_RemovesPunctuation()
    {
        var result = _parser.ParseWords("Hello, world! How are you?").ToList();
        
        Assert.Equal(new[] { "hello", "world", "how", "are", "you" }, result);
    }

    [Fact]
    public void ParseWords_EmptyString_ReturnsEmpty()
    {
        var result = _parser.ParseWords("").ToList();
        
        Assert.Empty(result);
    }

    [Fact]
    public void ParseWords_WhitespaceOnly_ReturnsEmpty()
    {
        var result = _parser.ParseWords("   \t\n  ").ToList();
        
        Assert.Empty(result);
    }

    [Fact]
    public void ParseWords_MixedCase_ReturnsLowercase()
    {
        var result = _parser.ParseWords("Hello WORLD Test").ToList();
        
        Assert.Equal(new[] { "hello", "world", "test" }, result);
    }

    [Fact]
    public void ParseWords_WithBrackets_RemovesBrackets()
    {
        var result = _parser.ParseWords("word1 [word2] (word3) {word4}").ToList();
        
        Assert.Equal(new[] { "word1", "word2", "word3", "word4" }, result);
    }

    [Fact]
    public void ParseWords_WithQuotes_RemovesQuotes()
    {
        var result = _parser.ParseWords("'hello' \"world\" test").ToList();
        
        Assert.Equal(new[] { "hello", "world", "test" }, result);
    }

    [Fact]
    public void ParseWords_WithNewlines_HandlesCorrectly()
    {
        var result = _parser.ParseWords("word1\nword2\r\nword3").ToList();
        
        Assert.Equal(new[] { "word1", "word2", "word3" }, result);
    }
}
