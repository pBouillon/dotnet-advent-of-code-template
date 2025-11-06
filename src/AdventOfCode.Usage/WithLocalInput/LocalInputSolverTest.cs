using AdventOfCode.Commons;

namespace AdventOfCode.Usage.WithLocalInput;

/// <summary>
/// Example of how to test the <see cref="Solver{TInput, TResult}"/>
/// 
/// For each part of the puzzle, we can specify what the example is and what solution it as, along with the
/// expected solution for our puzzle input
/// </summary>
public sealed class LocalInputSolverTest
    : TestEngine<Solver, int[], int>
{
    public override Puzzle PartOne => PuzzleBuilder
        .FromInput(["1", "2", "3"])
        .ParsedAs([1, 2, 3])
        .ExpectsResult(3)
        .WithTheActualSolutionBeing(5);

    public override Puzzle PartTwo => PuzzleBuilder
        .FromParsedInput([1, 2, 3])
        .ExpectsResult(6)
        .WithTheActualSolutionBeing(15);
}
