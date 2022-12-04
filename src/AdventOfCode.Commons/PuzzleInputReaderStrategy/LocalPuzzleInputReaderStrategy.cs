namespace AdventOfCode.Commons.PuzzleInputReaderStrategy;

/// <summary>
/// Strategy to retrieve the input from a local file at a given path
/// </summary>
public class LocalPuzzleInputReaderStrategy : IPuzzleInputReaderStrategy
{
    /// <summary>
    /// The path where the puzzle input is located
    /// </summary>
    public required string InputPath { get; init; }

    /// <inheritdoc />
    public IEnumerable<string> ReadInput()
        => File.ReadAllLines(InputPath);
}
