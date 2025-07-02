using System.Text;
using WordCounter.Interfaces;

namespace WordCounter.Services;

// Responsible only for extracting and normalizing words from character streams
// Removes punctuation, normalizes case, and handles word boundaries
public class WordParser : IWordParser
{
    // Processes characters and yields clean, normalized words
    public async IAsyncEnumerable<string> ParseWordsAsync(IAsyncEnumerable<char> characters)
    {
        var wordBuilder = new StringBuilder();

        await foreach (var character in characters)
        {
            if (IsWordSeparator(character))
            {
                var word = ExtractWordIfComplete(wordBuilder);
                if (word != null)
                {
                    yield return word;
                }
            }
            else if (IsWordCharacter(character))
            {
                AddLetterToWord(wordBuilder, character);
            }
        }

        // Handle the final word if the stream doesn't end with whitespace
        var finalWord = ExtractWordIfComplete(wordBuilder);
        if (finalWord != null)
        {
            yield return finalWord;
        }
    }

    // Determines if a character separates words (any whitespace)
    private static bool IsWordSeparator(char character)
    {
        return char.IsWhiteSpace(character);
    }

    // Determines if a character is a valid word character (letter or digit)
    private static bool IsWordCharacter(char character)
    {
        return char.IsLetter(character) || char.IsDigit(character);
    }

    // Extracts a completed word if available and clears the builder
    private static string? ExtractWordIfComplete(StringBuilder wordBuilder)
    {
        if (wordBuilder.Length == 0)
            return null;

        var word = wordBuilder.ToString().ToLowerInvariant();
        wordBuilder.Clear();
        return word;
    }

    // Adds a letter to the current word being built
    private static void AddLetterToWord(StringBuilder wordBuilder, char character)
    {
        wordBuilder.Append(character);
    }
}
