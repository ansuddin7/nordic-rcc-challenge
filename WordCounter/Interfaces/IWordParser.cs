namespace WordCounter.Interfaces;

// Responsible for extracting and normalizing words from text
public interface IWordParser
{
    // Extracts clean words from a character stream, removing punctuation and normalizing case
    IAsyncEnumerable<string> ParseWordsAsync(IAsyncEnumerable<char> characters);
}
