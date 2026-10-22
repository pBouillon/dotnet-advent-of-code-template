namespace AdventOfCode.Commons;

/// <summary>
/// Reads a puzzle input from a local file, downloading it first if it is missing and the puzzle is known.
/// Downloaded inputs are stored under <c>Inputs/</c> in the project directory and never fetched twice.
/// </summary>
internal sealed class PuzzleInputReader
{
    /// <summary>
    /// Serializes downloads, so the server is never hit in parallel and each file is written once.
    /// </summary>
    private static readonly Lock DownloadLock = new();

    /// <summary>
    /// The input path as given, resolved against the project directory when the input is read.
    /// </summary>
    private readonly string _relativePath;

    /// <summary>
    /// The source file of the solver, used to locate its project directory.
    /// </summary>
    private readonly string _solverSourcePath;

    /// <summary>
    /// The puzzle to download if the file is missing, or <c>null</c> if there is nothing to download.
    /// </summary>
    private readonly (int Year, int Day)? _puzzle;

    /// <summary>
    /// Creates a reader for <paramref name="relativePath"/>, located from the solver's source file.
    /// </summary>
    private PuzzleInputReader(string relativePath, (int Year, int Day)? puzzle, string solverSourcePath)
    {
        _relativePath = relativePath;
        _puzzle = puzzle;
        _solverSourcePath = solverSourcePath;
    }

    /// <summary>
    /// Creates a reader for an input that only exists as a local file.
    /// </summary>
    public static PuzzleInputReader ForFile(string inputPath, string solverSourcePath)
        => new(inputPath, puzzle: null, solverSourcePath);

    /// <summary>
    /// Creates a reader for a puzzle whose input is downloaded once, then read from <c>Inputs/</c>.
    /// </summary>
    public static PuzzleInputReader ForPuzzle(int year, int day, string solverSourcePath)
        => new(Path.Combine("Inputs", year.ToString(), $"day{day:00}.txt"), (year, day), solverSourcePath);

    /// <summary>
    /// Returns the lines of the input, downloading it first if needed.
    /// </summary>
    /// <exception cref="PuzzleInputUnavailableException">There is no local file and nothing to download.</exception>
    public IEnumerable<string> ReadInput()
    {
        var path = Path.GetFullPath(_relativePath, FindProjectDirectory(_solverSourcePath));

        if (!File.Exists(path))
        {
            Download(path);
        }

        return File.ReadAllLines(path);
    }

    /// <summary>
    /// Downloads the input into <paramref name="path"/>, unless a concurrent caller already did.
    /// </summary>
    /// <exception cref="PuzzleInputUnavailableException">There is nothing to download.</exception>
    private void Download(string path)
    {
        if (_puzzle is not { } puzzle)
        {
            throw new PuzzleInputUnavailableException($"No puzzle input found at '{path}'.");
        }

        lock (DownloadLock)
        {
            if (File.Exists(path)) return;

            var content = AdventOfCodeClient.DownloadInput(puzzle.Year, puzzle.Day);

            Directory.CreateDirectory(Path.GetDirectoryName(path)!);

            var temporaryPath = $"{path}.{Guid.NewGuid():N}.tmp";
            File.WriteAllText(temporaryPath, content);
            File.Move(temporaryPath, path, overwrite: true);
        }
    }

    /// <summary>
    /// Walks up from the solver's source file to the first folder holding a project file.
    /// Falls back to the output folder if there is none, e.g. when source paths were remapped.
    /// </summary>
    private static string FindProjectDirectory(string solverSourcePath)
    {
        var start = Path.GetDirectoryName(solverSourcePath);

        for (var directory = string.IsNullOrEmpty(start) ? null : new DirectoryInfo(start);
             directory is not null;
             directory = directory.Parent)
        {
            if (directory.Exists && directory.GetFiles("*.csproj").Length > 0) return directory.FullName;
        }

        return AppContext.BaseDirectory;
    }
}
