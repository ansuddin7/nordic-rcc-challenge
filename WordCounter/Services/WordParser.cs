using WordCounter.Interfaces;

namespace WordCounter.Services;

// Parses words from text using simple string splitting for performance
public class WordParser : IWordParser
{
    // Common word separators including punctuation and whitespace
    private static readonly char[] WordSeparators = { ' ', '\t', '\n', '\r', '.', ',', ';', ':', '!', '?', '"', '\'', '(', ')', '[', ']', '{', '}' };

    // Splits text into normalized words
    public IEnumerable<string> ParseWords(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
            return Enumerable.Empty<string>();

        return line
            .Split(WordSeparators, StringSplitOptions.RemoveEmptyEntries)
            .Select(word => word.ToLowerInvariant())
            .Where(word => !string.IsNullOrEmpty(word));
    }
}
