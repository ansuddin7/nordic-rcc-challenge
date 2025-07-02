using WordCounter.Interfaces;

namespace WordCounter.Services;

// Main orchestrator that coordinates all components to process multiple files in parallel
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

    // Processes multiple files in parallel and aggregates the word count results
    // Each file is processed as a separate task for optimal performance
    public async Task<Dictionary<string, int>> CountWordsInFilesAsync(IEnumerable<string> filePaths)
    {
        if (filePaths == null)
            throw new ArgumentNullException(nameof(filePaths));

        var validFilePaths = filePaths.Where(path => !string.IsNullOrWhiteSpace(path)).ToList();
        
        if (!validFilePaths.Any())
            return new Dictionary<string, int>();

        // Create a task for each file to process them in parallel
        var wordCountTasks = validFilePaths.Select(async filePath =>
        {
            var lines = _fileReader.ReadLinesAsync(filePath);
            return await _wordCounter.CountWordsAsync(lines);
        });

        // Aggregate all results
        return await _wordAggregator.AggregateAsync(wordCountTasks);
    }
}
