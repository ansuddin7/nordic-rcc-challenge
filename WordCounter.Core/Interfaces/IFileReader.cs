namespace WordCounter.Core.Interfaces;

public interface IFileReader
{
    IAsyncEnumerable<string> ReadLinesAsync(string filePath);
    Task<bool> FileExistsAsync(string filePath);
}
