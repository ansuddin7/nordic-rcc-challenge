namespace WordCounter.Interfaces;

public interface IFileReader
{
    IAsyncEnumerable<string> ReadLinesAsync(string filePath);
    Task<bool> FileExistsAsync(string filePath);
}
