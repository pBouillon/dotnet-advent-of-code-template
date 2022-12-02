using AdventOfCode.Commons;

namespace AdventOfCode.Usage;

/// <summary>
/// Example of how to test the <see cref="Solver{TInput, TResult}"/>
/// 
/// For each part of the puzzle, we can specify what the example is and what solution it as, along with the
/// expected solution for our puzzle input
/// </summary>
public class SolverTest : TestEngine<Solver, int[], int>
{
    public override Puzzle PartOne => new()
    {
        Example = new()
        {
            Input = new[] { 1, 2, 3 },
            Result = 3,
        },

        Solution = 5,
    };

    public override Puzzle PartTwo => new()
    {
        Example = new()
        {
            Input = new[] { 1, 2, 3 },
            Result = 6,
        },

        Solution = 15,
    };
}
