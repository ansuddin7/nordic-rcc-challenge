namespace WordCounter.Core.Interfaces;

public interface IWordParser
{
    IEnumerable<string> ParseWords(string line);
}
