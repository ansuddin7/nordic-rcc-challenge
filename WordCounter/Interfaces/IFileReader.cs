namespace WordCounter.Interfaces;

// Responsible only for reading files character by character
public interface IFileReader
{
    // Streams characters from a file for efficient processing of large files
    IAsyncEnumerable<char> ReadCharactersAsync(string filePath);
    
    // Checks if a file exists
    Task<bool> FileExistsAsync(string filePath);
}
