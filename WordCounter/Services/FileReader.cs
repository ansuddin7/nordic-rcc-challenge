using System.Runtime.CompilerServices;
using WordCounter.Interfaces;

namespace WordCounter.Services;

// Reads files efficiently using streaming to minimize memory usage
public class FileReader : IFileReader
{
    // Streams file content line by line to handle large files without loading everything into memory
    public async IAsyncEnumerable<string> ReadLinesAsync(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}", filePath);

        using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var reader = new StreamReader(fileStream);

        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            yield return line;
        }
    }

    public Task<bool> FileExistsAsync(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

        return Task.FromResult(File.Exists(filePath));
    }
}
