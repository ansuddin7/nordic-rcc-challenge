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
    public async Task ReadCharactersAsync_SimpleText_ReturnsAllCharacters()
    {
        var testFile = Path.Combine(_tempDirectory, "test.txt");
        var content = "Hello World";
        await File.WriteAllTextAsync(testFile, content);

        var actualCharacters = new List<char>();
        await foreach (var character in _fileReader.ReadCharactersAsync(testFile))
            actualCharacters.Add(character);

        Assert.Equal(content.ToCharArray(), actualCharacters);
    }

    [Fact]
    public async Task ReadCharactersAsync_EmptyFile_ReturnsNoCharacters()
    {
        var testFile = Path.Combine(_tempDirectory, "empty.txt");
        await File.WriteAllTextAsync(testFile, "");

        var actualCharacters = new List<char>();
        await foreach (var character in _fileReader.ReadCharactersAsync(testFile))
            actualCharacters.Add(character);

        Assert.Empty(actualCharacters);
    }

    [Fact]
    public async Task ReadCharactersAsync_TextWithPunctuation_ReturnsAllCharacters()
    {
        var testFile = Path.Combine(_tempDirectory, "test.txt");
        var content = "Hello, world! How are you?";
        await File.WriteAllTextAsync(testFile, content);

        var actualCharacters = new List<char>();
        await foreach (var character in _fileReader.ReadCharactersAsync(testFile))
            actualCharacters.Add(character);

        Assert.Equal(content.ToCharArray(), actualCharacters);
    }

    [Fact]
    public async Task ReadCharactersAsync_OnlyWhitespace_ReturnsWhitespaceCharacters()
    {
        var testFile = Path.Combine(_tempDirectory, "test.txt");
        var content = "   \t\n  \r\n  ";
        await File.WriteAllTextAsync(testFile, content);

        var actualCharacters = new List<char>();
        await foreach (var character in _fileReader.ReadCharactersAsync(testFile))
            actualCharacters.Add(character);

        Assert.Equal(content.ToCharArray(), actualCharacters);
    }

    [Fact]
    public async Task ReadCharactersAsync_MultilineText_ReturnsAllCharactersIncludingNewlines()
    {
        var testFile = Path.Combine(_tempDirectory, "test.txt");
        var content = "Line 1\nLine 2\r\nLine 3";
        await File.WriteAllTextAsync(testFile, content);

        var actualCharacters = new List<char>();
        await foreach (var character in _fileReader.ReadCharactersAsync(testFile))
            actualCharacters.Add(character);

        Assert.Equal(content.ToCharArray(), actualCharacters);
    }

    [Fact]
    public async Task ReadCharactersAsync_LargeFile_HandlesEfficiently()
    {
        var testFile = Path.Combine(_tempDirectory, "large.txt");
        var content = string.Join(" ", Enumerable.Repeat("word", 10000));
        await File.WriteAllTextAsync(testFile, content);

        var characterCount = 0;
        await foreach (var character in _fileReader.ReadCharactersAsync(testFile))
            characterCount++;

        Assert.Equal(content.Length, characterCount);
    }

    [Fact]
    public async Task ReadCharactersAsync_NonExistentFile_ThrowsFileNotFoundException()
    {
        var nonExistentFile = Path.Combine(_tempDirectory, "nonexistent.txt");

        await Assert.ThrowsAsync<FileNotFoundException>(async () =>
        {
            await foreach (var character in _fileReader.ReadCharactersAsync(nonExistentFile)) { }
        });
    }

    [Fact]
    public async Task ReadCharactersAsync_NullFilePath_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await foreach (var character in _fileReader.ReadCharactersAsync(null!)) { }
        });
    }

    [Fact]
    public async Task ReadCharactersAsync_EmptyFilePath_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await foreach (var character in _fileReader.ReadCharactersAsync("")) { }
        });
    }

    [Fact]
    public async Task FileExistsAsync_ExistingFile_ReturnsTrue()
    {
        var testFile = Path.Combine(_tempDirectory, "test.txt");
        await File.WriteAllTextAsync(testFile, "content");

        var exists = await _fileReader.FileExistsAsync(testFile);

        Assert.True(exists);
    }

    [Fact]
    public async Task FileExistsAsync_NonExistentFile_ReturnsFalse()
    {
        var nonExistentFile = Path.Combine(_tempDirectory, "nonexistent.txt");

        var exists = await _fileReader.FileExistsAsync(nonExistentFile);

        Assert.False(exists);
    }

    [Fact]
    public async Task FileExistsAsync_NullFilePath_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _fileReader.FileExistsAsync(null!));
    }
}
