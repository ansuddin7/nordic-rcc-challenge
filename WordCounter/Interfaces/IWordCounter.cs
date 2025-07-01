namespace WordCounter.Core.Interfaces;

public interface IWordCounter
{
    Task<Dictionary<string, int>> CountWordsAsync(IAsyncEnumerable<string> lines);
}
