using Shouldly;

using Xunit;

namespace AdventOfCode.Commons;

public abstract partial class TestEngine<TSolver, TInput, TResult>
    where TSolver : Solver<TInput, TResult>, new()
{
    [Fact(DisplayName = "Part Two - Parsing")]
    public void PartTwoParsingTest()
    {
        var shouldBeSkipped = PartTwo.ShouldSkipTests
          || PartTwo.Example.RawInput.Length == 0;

        Assert.SkipWhen(shouldBeSkipped, "Puzzle.ShouldSkipTests has been set to true or no raw input provided, test skipped");

        // Arrange
        var input = PartTwo.Example.RawInput;

        // Act
        var result = _solver.ParseInput(input);

        // Assert
        result.ShouldBeEquivalentTo(PartTwo.Example.Input);
    }

    [Fact(DisplayName = "Part Two - Example")]
    public void PartTwoExampleTest()
    {
        Assert.SkipWhen(PartTwo.ShouldSkipTests, "Puzzle.ShouldSkipTests has been set to true, test skipped");

        // Arrange
        var input = PartTwo.Example.Input;

        // Act
        var result = _solver.PartTwo(input);

        // Assert
        result.ShouldBe(PartTwo.Example.Result);
    }

    [Fact(DisplayName = "Part Two - Solution")]
    public void PartTwoSolutionTest()
    {
        Assert.SkipWhen(PartTwo.ShouldSkipTests, "Puzzle.ShouldSkipTests has been set to true, test skipped");

        // Arrange
        var input = GetPuzzleInputOrSkip();

        // Act
        var result = _solver.PartTwo(input);

        // Assert
        result.ShouldBe(PartTwo.Solution);
    }
}
