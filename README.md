# Word Counter

Counts word occurrences across multiple text files with parallel processing.

## How to Run

```bash
dotnet run --project WordCounter.Core
```

Processes all `.txt` files in the `SampleFiles/` directory.

## Run Tests

```bash
dotnet test
```

## Solution
The solution follows SOLID principles with these main components:
- **IFileReader**: Streams file content efficiently
- **IWordParser**: Extracts and normalizes words
- **IWordCounter**: Counts word occurrences per file
- **IWordAggregator**: Combines results from multiple files using thread-safe operations
- **IWordCounterService**: Orchestrates the entire process
