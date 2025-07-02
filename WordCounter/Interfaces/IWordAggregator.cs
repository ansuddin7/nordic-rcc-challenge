namespace WordCounter.Interfaces;

public interface IWordAggregator
{
    Task<Dictionary<string, int>> AggregateAsync(IEnumerable<Task<Dictionary<string, int>>> wordCountTasks);
}
