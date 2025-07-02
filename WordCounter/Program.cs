using Microsoft.Extensions.DependencyInjection;
using WordCounter.Interfaces;
using WordCounter.Services;

var services = new ServiceCollection();
services.AddTransient<IFileReader, FileReader>();
services.AddTransient<IWordParser, WordParser>();
services.AddTransient<IWordCounter, WordCounter.Services.WordCounter>();
services.AddTransient<IWordAggregator, WordAggregator>();
services.AddTransient<IWordCounterService, WordCounterService>();

var serviceProvider = services.BuildServiceProvider();
var wordCounterService = serviceProvider.GetRequiredService<IWordCounterService>();

await ProcessFilesAsync(wordCounterService);

serviceProvider.Dispose();

static async Task ProcessFilesAsync(IWordCounterService wordCounterService)
{
    var filePaths = Directory.GetFiles("SampleFiles", "*.txt");
    
    var results = await wordCounterService.CountWordsInFilesAsync(filePaths);
    
    foreach (var kvp in results.OrderByDescending(x => x.Value))
    {
        Console.WriteLine($"{kvp.Key}: {kvp.Value}");
    }
}
