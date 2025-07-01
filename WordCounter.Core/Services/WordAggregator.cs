using System.Collections.Concurrent;
using WordCounter.Core.Interfaces;

namespace WordCounter.Core.Services;

public class WordAggregator : IWordAggregator
{
    public async Task<Dictionary<string, int>> AggregateAsync(IEnumerable<Task<Dictionary<string, int>>> wordCountTasks)
    {
        var results = await Task.WhenAll(wordCountTasks);
        var aggregatedCounts = new ConcurrentDictionary<string, int>();

        Parallel.ForEach(results, wordCounts =>
        {
            foreach (var kvp in wordCounts)
            {
                aggregatedCounts.AddOrUpdate(
                    kvp.Key,
                    kvp.Value,
                    (key, existingValue) => existingValue + kvp.Value);
            }
        });

        return new Dictionary<string, int>(aggregatedCounts);
    }
}
