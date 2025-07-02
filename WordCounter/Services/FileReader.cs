using WordCounter.Interfaces;

namespace WordCounter.Services;

// Responsible only for reading files character by character using streaming for memory efficiency
public class FileReader : IFileReader
{
    private const int BufferSize = 8192; // 8KB buffer for efficient I/O

    // Streams characters from a file to handle extremely large files efficiently
    public async IAsyncEnumerable<char> ReadCharactersAsync(string filePath)
    {
        ValidateFilePath(filePath);
        
        using var reader = CreateStreamReader(filePath);

        await foreach (var character in ReadCharactersFromStreamAsync(reader))
        {
            yield return character;
        }
    }

    public Task<bool> FileExistsAsync(string filePath)
    {
        ValidateFilePath(filePath);
        return Task.FromResult(File.Exists(filePath));
    }

    // Validates that the file path is not null or empty
    private static void ValidateFilePath(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
    }

    // Creates a StreamReader with optimal settings for reading large files
    private static StreamReader CreateStreamReader(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}", filePath);

        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return new StreamReader(fileStream);
    }

    // Reads characters from the stream using a buffer for improved efficiency
    private static async IAsyncEnumerable<char> ReadCharactersFromStreamAsync(StreamReader reader)
    {
        var buffer = new char[BufferSize];
        int charsRead;
        while ((charsRead = await reader.ReadAsync(buffer, 0, BufferSize)) > 0)
        {
            for (int i = 0; i < charsRead; i++)
            {
                yield return buffer[i];
            }
        }
    }
}
