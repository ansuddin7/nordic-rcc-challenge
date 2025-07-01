using WordCounter.Core.Interfaces;

namespace WordCounter.Core.Services;

public class WordCounter : IWordCounter
{
    private readonly IWordParser _wordParser;

    public WordCounter(IWordParser wordParser)
    {
        _wordParser = wordParser ?? throw new ArgumentNullException(nameof(wordParser));
    }

    public async Task<Dictionary<string, int>> CountWordsAsync(IAsyncEnumerable<string> lines)
    {
        var wordCounts = new Dictionary<string, int>();

        await foreach (var line in lines)
        {
            var words = _wordParser.ParseWords(line);
            
            foreach (var word in words)
            {
                wordCounts[word] = wordCounts.GetValueOrDefault(word, 0) + 1;
            }
        }

        return wordCounts;
    }
}
