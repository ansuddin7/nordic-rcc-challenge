namespace WordCounter.Core.Interfaces;

public interface IWordCounterService
{
    Task<Dictionary<string, int>> CountWordsInFilesAsync(IEnumerable<string> filePaths);
}
