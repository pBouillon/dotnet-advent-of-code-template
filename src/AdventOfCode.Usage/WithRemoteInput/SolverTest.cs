using AdventOfCode.Commons;

namespace AdventOfCode.Usage.WithRemoteInput;

public sealed class SolverTest : TestEngine<Solver, int[], int>
{
    public override Puzzle PartOne => new()
    {
        // Once your cookie set in Configuration.Cookie, you can safely toggle this flag
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
        // Once your cookie set in Configuration.Cookie, you can safely toggle this flag
        ShouldSkipTests = true,

        Example = new()
        {
            Input = [199, 200, 208, 210, 200, 207, 240, 269, 260, 263],
            Result = 5,
        },
        Solution = 1737,
    };
}
