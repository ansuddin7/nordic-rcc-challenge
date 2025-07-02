namespace WordCounter.Interfaces;

public interface IWordCounterService
{
    Task<Dictionary<string, int>> CountWordsInFilesAsync(IEnumerable<string> filePaths);
}
