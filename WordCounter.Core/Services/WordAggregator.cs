using System.Collections.Concurrent;
using WordCounter.Core.Interfaces;

namespace WordCounter.Core.Services;

// Aggregates word counts from multiple files using thread-safe operations
// Uses ConcurrentDictionary to safely merge results from parallel file processing
public class WordAggregator : IWordAggregator
{
    // Waits for all file processing tasks to complete, then merges results in parallel
    public async Task<Dictionary<string, int>> AggregateAsync(IEnumerable<Task<Dictionary<string, int>>> wordCountTasks)
    {
        // Wait for all file processing to complete
        var results = await Task.WhenAll(wordCountTasks);
        
        // Use thread-safe dictionary for parallel aggregation
        var aggregatedCounts = new ConcurrentDictionary<string, int>();

        // Process each file's results in parallel
        Parallel.ForEach(results, wordCounts =>
        {
            foreach (var kvp in wordCounts)
            {
                // Add or update word count
                aggregatedCounts.AddOrUpdate(
                    kvp.Key,
                    kvp.Value,
                    (key, existingValue) => existingValue + kvp.Value);
            }
        });

        // Return regular dictionary for final result
        return new Dictionary<string, int>(aggregatedCounts);
    }
}
