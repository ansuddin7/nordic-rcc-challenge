namespace WordCounter.Interfaces;

public interface IWordCounter
{
    Task<Dictionary<string, int>> CountWordsAsync(IAsyncEnumerable<string> words);
}
