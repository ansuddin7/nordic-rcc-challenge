using WordCounter.Services;

namespace WordCounter.Tests;

public class FileReaderTests : IDisposable
{
    private readonly string _tempDirectory;
    private readonly FileReader _fileReader;

    public FileReaderTests()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_tempDirectory);
        _fileReader = new FileReader();
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDirectory))
            Directory.Delete(_tempDirectory, true);
    }

    [Fact]
    public async Task ReadLinesAsync_ValidFile_ReturnsAllLines()
    {
        var testFile = Path.Combine(_tempDirectory, "test.txt");
        var expectedLines = new[] { "Line 1", "Line 2", "Line 3" };
        await File.WriteAllLinesAsync(testFile, expectedLines);

        var actualLines = new List<string>();
        await foreach (var line in _fileReader.ReadLinesAsync(testFile))
            actualLines.Add(line);

        Assert.Equal(expectedLines, actualLines);
    }

    [Fact]
    public async Task ReadLinesAsync_EmptyFile_ReturnsNoLines()
    {
        var testFile = Path.Combine(_tempDirectory, "empty.txt");
        await File.WriteAllTextAsync(testFile, string.Empty);

        var actualLines = new List<string>();
        await foreach (var line in _fileReader.ReadLinesAsync(testFile))
            actualLines.Add(line);

        Assert.Empty(actualLines);
    }

    [Fact]
    public async Task ReadLinesAsync_NonExistentFile_ThrowsFileNotFoundException()
    {
        var nonExistentFile = Path.Combine(_tempDirectory, "nonexistent.txt");

        await Assert.ThrowsAsync<FileNotFoundException>(async () =>
        {
            await foreach (var line in _fileReader.ReadLinesAsync(nonExistentFile)) { }
        });
    }

    [Fact]
    public async Task FileExistsAsync_ExistingFile_ReturnsTrue()
    {
        var testFile = Path.Combine(_tempDirectory, "exists.txt");
        await File.WriteAllTextAsync(testFile, "content");

        var result = await _fileReader.FileExistsAsync(testFile);

        Assert.True(result);
    }

    [Fact]
    public async Task FileExistsAsync_NonExistentFile_ReturnsFalse()
    {
        var nonExistentFile = Path.Combine(_tempDirectory, "nonexistent.txt");

        var result = await _fileReader.FileExistsAsync(nonExistentFile);

        Assert.False(result);
    }
}
