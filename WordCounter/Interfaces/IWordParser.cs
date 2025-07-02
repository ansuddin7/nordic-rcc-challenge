namespace WordCounter.Interfaces;

public interface IWordParser
{
    IEnumerable<string> ParseWords(string line);
}
