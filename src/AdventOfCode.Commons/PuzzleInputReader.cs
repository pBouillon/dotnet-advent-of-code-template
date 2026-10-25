namespace AdventOfCode.Commons;

/// <summary>
/// Reads a puzzle input from a file next to the solver, downloading it first if it is missing and the puzzle is known.
/// Downloaded inputs are written next to the solver and never fetched twice.
/// </summary>
internal sealed class PuzzleInputReader
{
    /// <summary>
    /// Serializes downloads, so the server is never hit in parallel and each file is written once.
    /// </summary>
    private static readonly Lock DownloadLock = new();

    /// <summary>
    /// The absolute path of the input file.
    /// </summary>
    private readonly string _path;

    /// <summary>
    /// The puzzle to download if the file is missing, or <c>null</c> if there is nothing to download.
    /// </summary>
    private readonly (int Year, int Day)? _puzzle;

    /// <summary>
    /// Creates a reader for the input file at <paramref name="path"/>.
    /// </summary>
    private PuzzleInputReader(string path, (int Year, int Day)? puzzle)
    {
        _path = path;
        _puzzle = puzzle;
    }

    /// <summary>
    /// Creates a reader for an input that only exists as a file, relative to the solver's folder.
    /// </summary>
    public static PuzzleInputReader ForFile(string inputPath, string solverSourcePath)
        => new(Resolve(inputPath, solverSourcePath), puzzle: null);

    /// <summary>
    /// Creates a reader for a puzzle whose input is downloaded once, next to the solver, then read from there.
    /// </summary>
    public static PuzzleInputReader ForPuzzle(int year, int day, string solverSourcePath)
        => new(Resolve($"input.{year}-day{day:00}.txt", solverSourcePath), (year, day));

    /// <summary>
    /// Returns the lines of the input, downloading it first if needed.
    /// </summary>
    /// <exception cref="PuzzleInputUnavailableException">There is no file and nothing to download.</exception>
    public IEnumerable<string> ReadInput()
    {
        if (!File.Exists(_path))
        {
            Download();
        }

        return File.ReadAllLines(_path);
    }

    /// <summary>
    /// Downloads the input into the input file, unless a concurrent caller already did.
    /// </summary>
    /// <exception cref="PuzzleInputUnavailableException">There is nothing to download.</exception>
    private void Download()
    {
        if (_puzzle is not { } puzzle)
        {
            throw new PuzzleInputUnavailableException($"No puzzle input found at '{_path}'.");
        }

        lock (DownloadLock)
        {
            if (File.Exists(_path)) return;

            var content = AdventOfCodeClient.DownloadInput(puzzle.Year, puzzle.Day);

            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);

            var temporaryPath = $"{_path}.{Guid.NewGuid():N}.tmp";
            File.WriteAllText(temporaryPath, content);
            File.Move(temporaryPath, _path, overwrite: true);
        }
    }

    /// <summary>
    /// Resolves <paramref name="path"/> against the folder of the solver's source file.
    /// </summary>
    private static string Resolve(string path, string solverSourcePath)
        => Path.GetFullPath(path, Path.GetDirectoryName(solverSourcePath) ?? AppContext.BaseDirectory);
}
