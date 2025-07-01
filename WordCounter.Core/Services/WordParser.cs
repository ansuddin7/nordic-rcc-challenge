using WordCounter.Core.Interfaces;

namespace WordCounter.Core.Services;

public class WordParser : IWordParser
{
    private static readonly char[] WordSeparators = { ' ', '\t', '\n', '\r', '.', ',', ';', ':', '!', '?', '"', '\'', '(', ')', '[', ']', '{', '}' };

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
