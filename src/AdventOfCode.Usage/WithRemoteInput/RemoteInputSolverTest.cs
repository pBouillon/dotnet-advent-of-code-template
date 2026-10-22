using AdventOfCode.Commons;

namespace AdventOfCode.Usage.WithRemoteInput;

/// <summary>
/// Example of how to test a <see cref="Solver"/> whose input is downloaded from the website
///
/// Its tests are skipped so that the template never downloads an input
/// </summary>
public sealed class RemoteInputSolverTest
    : TestEngine<Solver, int[], int>
{
    public override Puzzle PartOne => new()
    {
        // Once your cookie is set (see the README) and the Solution below is yours, you can safely remove this flag
        ShouldSkipTests = true,

        Example = new()
        {
            Input = [199, 200, 208, 210, 200, 207, 240, 269, 260, 263],
            Result = 7,
        },
        Solution = 1696,
    };

    public override Puzzle PartTwo => new()
    {
        // Once your cookie is set (see the README) and the Solution below is yours, you can safely remove this flag
        ShouldSkipTests = true,

        Example = new()
        {
            Input = [199, 200, 208, 210, 200, 207, 240, 269, 260, 263],
            Result = 5,
        },
        Solution = 1737,
    };
}
