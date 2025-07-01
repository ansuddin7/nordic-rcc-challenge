using WordCounter.Core.Interfaces;

namespace WordCounter.Core.Services;

public class WordCounterService : IWordCounterService
{
    private readonly IFileReader _fileReader;
    private readonly IWordCounter _wordCounter;
    private readonly IWordAggregator _wordAggregator;

    public WordCounterService(
        IFileReader fileReader, 
        IWordCounter wordCounter, 
        IWordAggregator wordAggregator)
    {
        _fileReader = fileReader ?? throw new ArgumentNullException(nameof(fileReader));
        _wordCounter = wordCounter ?? throw new ArgumentNullException(nameof(wordCounter));
        _wordAggregator = wordAggregator ?? throw new ArgumentNullException(nameof(wordAggregator));
    }

    public async Task<Dictionary<string, int>> CountWordsInFilesAsync(IEnumerable<string> filePaths)
    {
        if (filePaths == null)
            throw new ArgumentNullException(nameof(filePaths));

        var validFilePaths = filePaths.Where(path => !string.IsNullOrWhiteSpace(path)).ToList();
        
        if (!validFilePaths.Any())
            return new Dictionary<string, int>();

        var wordCountTasks = validFilePaths.Select(async filePath =>
        {
            var lines = _fileReader.ReadLinesAsync(filePath);
            return await _wordCounter.CountWordsAsync(lines);
        });

        return await _wordAggregator.AggregateAsync(wordCountTasks);
    }
}
