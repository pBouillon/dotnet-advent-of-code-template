using Shouldly;
using Xunit;

namespace AdventOfCode.Commons;

public abstract partial class TestEngine<TSolver, TInput, TResult>
    where TSolver : Solver<TInput, TResult>, new()
{
    [SkippableFact(DisplayName = "Part Two - Parsing")]
    public void PartTwoParsingTest()
    {
        var shouldBeSkipped = PartTwo.ShouldSkipTests
          || PartTwo.Example.RawInput.Length == 0;

        Skip.If(shouldBeSkipped, "Puzzle.ShouldSkipTests has been set to true or no raw input provided, test skipped");

        // Arrange
        var input = PartTwo.Example.RawInput;

        // Act
        var result = _solver.ParseInput(input);

        // Assert
        result.ShouldBeEquivalentTo(PartTwo.Example.Input);
    }

    [SkippableFact(DisplayName = "Part Two - Example")]
    public void PartTwoExampleTest()
    {
        Skip.If(PartTwo.ShouldSkipTests, "Puzzle.ShouldSkipTests has been set to true, test skipped");

        // Arrange
        var input = PartTwo.Example.Input;

        // Act
        var result = _solver.PartTwo(input);

        // Assert
        result.ShouldBe(PartTwo.Example.Result);
    }

    [SkippableFact(DisplayName = "Part Two - Solution")]
    public void PartTwoSolutionTest()
    {
        Skip.If(PartTwo.ShouldSkipTests, "Puzzle.ShouldSkipTests has been set to true, test skipped");

        // Arrange
        var input = _solver.PuzzleInput;

        // Act
        var result = _solver.PartTwo(input.Value);

        // Assert
        result.ShouldBe(PartTwo.Solution);
    }
}