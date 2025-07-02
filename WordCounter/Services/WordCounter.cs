using WordCounter.Interfaces;

namespace WordCounter.Services;

// Counts word occurrences from a single file's content
// Uses regular Dictionary since this processes one file sequentially
public class WordCounter : IWordCounter
{
    private readonly IWordParser _wordParser;

    public WordCounter(IWordParser wordParser)
    {
        _wordParser = wordParser ?? throw new ArgumentNullException(nameof(wordParser));
    }

    // Processes lines asynchronously and counts each word occurrence
    public async Task<Dictionary<string, int>> CountWordsAsync(IAsyncEnumerable<string> lines)
    {
        var wordCounts = new Dictionary<string, int>();

        await foreach (var line in lines)
        {
            var words = _wordParser.ParseWords(line);
            
            // Increment count for each word
            foreach (var word in words)
            {
                wordCounts[word] = wordCounts.GetValueOrDefault(word, 0) + 1;
            }
        }

        return wordCounts;
    }
}
