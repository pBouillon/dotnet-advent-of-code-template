using AdventOfCode.Commons;

namespace AdventOfCode.Usage.WithRemoteInput;

public class SolverTest : TestEngine<Solver, int[], int>
{
    public override Puzzle PartOne => new()
    {
        Example = new()
        {
            Input = new[] { 199, 200, 208, 210, 200, 207, 240, 269, 260, 263 },
            Result = 7,
        },
        Solution = 1696,
    };

    public override Puzzle PartTwo => new()
    {
        Example = new()
        {
            Input = new[] { 199, 200, 208, 210, 200, 207, 240, 269, 260, 263 },
            Result = 5,
        },
        Solution = 1737,
    };
}
