using Xunit;

namespace AdventOfCode.Commons;

/// <summary>
/// Test engine for the implemented <typeparamref name="TSolver"/>
/// </summary>
public abstract partial class TestEngine<TSolver, TInput, TResult>
  where TSolver : Solver<TInput, TResult>, new()
{
    /// <summary>
    /// The solver to use to solve the puzzle
    /// </summary>

    private readonly TSolver _solver = new();

    public abstract Puzzle PartOne { get; }

    public abstract Puzzle PartTwo { get; }

    /// <summary>
    /// Returns the puzzle input, skipping the running test if it is unavailable.
    /// </summary>
    private TInput GetPuzzleInputOrSkip()
    {
        try
        {
            return _solver.PuzzleInput.Value;
        }
        catch (PuzzleInputUnavailableException exception)
        {
            Assert.Skip(exception.Message);
            throw;
        }
    }
}
