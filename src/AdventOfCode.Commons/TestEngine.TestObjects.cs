namespace AdventOfCode.Commons;

public abstract partial class TestEngine<TSolver, TInput, TResult>
    where TSolver : Solver<TInput, TResult>, new()
{
    #region Test objects
    /// <summary>
    /// The example used in the advent of code's subject and the expected result for it
    /// <para>
    /// This class is used to test your implementation of the first and second part of the puzzle
    /// </para>
    /// </summary>
    public record Example
    {
        /// <summary>
        /// The input you would like to test the parser with
        /// </summary>
        /// <remarks>
        /// Leave empty if yuo do not wish to test the parsing
        /// </remarks>
        public string[] RawInput { get; init; } = [];

        /// <summary>
        /// The data of the input mentioned in the example, in the same form as the
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
    #endregion

    #region Puzzle Builder
    public static class PuzzleBuilder
    {
        public static ParsedAsStep FromInput(string[] rawInput)
          => new(rawInput);

        public static ExpectsResultStep FromParsedInput(TInput parsed)
         => new(rawInput: [], parsed);
    }

    public class ParsedAsStep(string[] rawInput)
    {
        public ExpectsResultStep ParsedAs(TInput expectedParsedInput)
          => new(rawInput, expectedParsedInput);
    }

    public class ExpectsResultStep(string[] rawInput, TInput parsedInput)
    {
        public ActualSolutionStep ExpectsResult(TResult expectedResult)
        {
            var example = new Example
            {
                RawInput = rawInput,
                Input = parsedInput,
                Result = expectedResult
            };

            return new ActualSolutionStep(example);
        }
    }

    public class ActualSolutionStep(Example example)
    {
        public Puzzle WithTheActualSolutionBeing(TResult solution, bool skipTests = false)
          => new()
          {
              Example = example,
              Solution = solution,
              ShouldSkipTests = skipTests
          };
    }
    #endregion
}
