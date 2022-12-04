using FluentAssertions;

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
public abstract class TestEngine<TSolver, TInput, TResult>
    where TSolver : Solver<TInput, TResult>, new()
{
    /// <summary>
    /// The example used in the advent of code's subject and the expected result for it
    /// <para>
    /// This class is used to test your implementation of the first and second part of the puzzle
    /// </para>
    /// </summary>
    public record Example
    {
        /// <summary>
        /// The data of the input mentionned in the example, in the same form as the
        /// <see cref="Solver{TInput, TResult}.PuzzleInput"/>
        /// </summary>
        public TInput Input { get; init; } = default!;

        /// <summary>
        /// The expected result for this example
        /// </summary>
        public TResult Result { get; init; } = default!;
    }

    /// <summary>
    /// The inputs to solve the given puzzle along with the expected solution
    /// </summary>
    public record Puzzle
    {
        /// <summary>
        /// If set to <c>true</c>, the tests for this puzzle will be skipped
        /// </summary>
        /// <remarks>
        /// Default is <c>false</c>
        /// </remarks>
        public bool ShouldSkipTests { get; init; } = false;

        /// <summary>
        /// The <see cref="Example"/> to use to test <see cref="Solver{TInput, TResult}"/>
        /// </summary>
        public required Example Example { get; init; }

        /// <summary>
        /// The expected solution for the puzzle
        /// </summary>
        public required TResult Solution { get; init; }
    }

    /// <summary>
    /// The solver to use to solve the puzzle
    /// </summary>
    private readonly TSolver _solver;

    protected TestEngine() => _solver = new TSolver();

    #region Part #1

    public abstract Puzzle PartOne { get; }

    [SkippableFact(DisplayName = "Part One - Example")]
    public void PartOneExampleTest()
    {
        Skip.If(PartOne.ShouldSkipTests, "Puzzle.ShouldSkipTests has been set to true, test skipped");

        // Arrange
        var input = PartOne.Example.Input;

        // Act
        var result = _solver.PartOne(input);

        // Assert
        result.Should().Be(PartOne.Example.Result);
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
        result.Should().Be(PartOne.Solution);
    }

    #endregion

    #region Part #2

    public abstract Puzzle PartTwo { get; }

    [SkippableFact(DisplayName = "Part Two - Example")]
    public void PartTwoExampleTest()
    {
        Skip.If(PartTwo.ShouldSkipTests, "Puzzle.ShouldSkipTests has been set to true, test skipped");

        // Arrange
        var input = PartTwo.Example.Input;

        // Act
        var result = _solver.PartTwo(input);

        // Assert
        result.Should().Be(PartTwo.Example.Result);
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
        result.Should().Be(PartTwo.Solution);
    }

    #endregion
}
