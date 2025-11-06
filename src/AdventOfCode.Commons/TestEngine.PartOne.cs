using Shouldly;
using Xunit;

namespace AdventOfCode.Commons;

/// <summary>
/// Test engine for the implemented <typeparamref name="TSolver"/>
/// </summary>
///
/// <typeparam name="TSolver">
/// The <see cref="Solver{TInput, TResult}"/> to use
/// </typeparam>
///
/// <typeparam name="TInput">
/// The type of the data used to solve the puzzle
/// </typeparam>
///
/// <typeparam name="TResult">
/// The type of the result of the puzzle
/// </typeparam> 
public abstract partial class TestEngine<TSolver, TInput, TResult>
    where TSolver : Solver<TInput, TResult>, new()
{
    [SkippableFact(DisplayName = "Part One - Parsing")]
    public void PartOneParsingTest()
    {
        var shouldBeSkipped = PartOne.ShouldSkipTests
          || PartOne.Example.RawInput.Length == 0;

        Skip.If(shouldBeSkipped, "Puzzle.ShouldSkipTests has been set to true or no raw input provided, test skipped");

        // Arrange
        var input = PartOne.Example.RawInput;

        // Act
        var result = _solver.ParseInput(input);

        // Assert
        result.ShouldBeEquivalentTo(PartOne.Example.Input);
    }

    [SkippableFact(DisplayName = "Part One - Example")]
    public void PartOneExampleTest()
    {
        Skip.If(PartOne.ShouldSkipTests, "Puzzle.ShouldSkipTests has been set to true, test skipped");

        // Arrange
        var input = PartOne.Example.Input;

        // Act
        var result = _solver.PartOne(input);

        // Assert
        result.ShouldBe(PartOne.Example.Result);
    }

    [SkippableFact(DisplayName = "Part One - Solution")]
    public void PartOneSolutionTest()
    {
        Skip.If(PartOne.ShouldSkipTests, "Puzzle.ShouldSkipTests has been set to true, test skipped");

        // Arrange
        var input = _solver.PuzzleInput;

        // Act
        var result = _solver.PartOne(input.Value);

        // Assert
        result.ShouldBe(PartOne.Solution);
    }
}
