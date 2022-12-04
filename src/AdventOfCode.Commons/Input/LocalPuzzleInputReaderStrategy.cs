namespace AdventOfCode.Commons.Input;

public class LocalPuzzleInputReaderStrategy : IPuzzleInputReaderStrategy
{
    public required string InputPath { get; init; }

    public IEnumerable<string> ReadInput()
        => File.ReadAllLines(InputPath);
}
