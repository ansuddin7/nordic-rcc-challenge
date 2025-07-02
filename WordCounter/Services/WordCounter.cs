using WordCounter.Interfaces;

namespace WordCounter.Services;

// Counts word occurrences from a stream of pre-parsed words
// Uses regular Dictionary since this processes one file sequentially
public class WordCounter : IWordCounter
{
    // Processes words asynchronously and counts each occurrence
    public async Task<Dictionary<string, int>> CountWordsAsync(IAsyncEnumerable<string> words)
    {
        var wordCounts = CreateWordCountDictionary();

        await foreach (var word in words)
        {
            if (IsValidWord(word))
            {
                IncrementWordCount(wordCounts, word);
            }
        }

        return wordCounts;
    }

    // Creates a new dictionary for storing word counts
    private static Dictionary<string, int> CreateWordCountDictionary()
    {
        return new Dictionary<string, int>();
    }

    // Validates that a word is not null or empty
    private static bool IsValidWord(string word)
    {
        return !string.IsNullOrWhiteSpace(word);
    }

    // Increments the count for a specific word
    private static void IncrementWordCount(Dictionary<string, int> wordCounts, string word)
    {
        wordCounts[word] = wordCounts.GetValueOrDefault(word, 0) + 1;
    }
}
