using AdventOfCode.Commons;

namespace AdventOfCode.Usage;

/// <summary>
/// Example of how to use the <see cref="Solver{TInput, TResult}"/> for a simple puzzle
/// 
/// In this example, the input is a succession of integers on different lines
/// 
/// The first part aims to find the biggest number
/// 
/// The second part aims to find the sum of all the numbers
/// </summary>
public class Solver : Solver<int[], int>
{
    protected override string InputPath => "input.txt";

    public override int PartOne(int[] input)
        => input.Max();

    public override int PartTwo(int[] input)
        => input.Sum();

    public override int[] ReadInput(string inputPath)
        => File
            .ReadAllLines(inputPath)
            .Select(int.Parse)
            .ToArray();
}
