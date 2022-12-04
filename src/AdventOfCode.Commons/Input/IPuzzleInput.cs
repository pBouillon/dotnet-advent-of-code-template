namespace AdventOfCode.Commons.Input;

public interface IPuzzleInputReaderStrategy
{
    IEnumerable<string> ReadInput();
}
