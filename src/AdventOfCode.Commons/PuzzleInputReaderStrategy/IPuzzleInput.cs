namespace AdventOfCode.Commons.PuzzleInputReaderStrategy;

/// <summary>
/// Defines the strategy to be used to read the puzzle input
/// </summary>
public interface IPuzzleInputReaderStrategy
{
    /// <summary>
    /// Read the lines composing puzzle input
    /// </summary>
    ///
    /// <returns>
    /// The puzzle input as an enumeration of its lines
    /// </returns>
    ///
    /// <remarks>
    /// The trailing blank line is omitted
    /// </remarks>
    IEnumerable<string> ReadInput();
}
