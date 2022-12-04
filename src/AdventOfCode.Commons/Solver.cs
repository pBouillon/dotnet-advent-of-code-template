using AdventOfCode.Commons.Input;

namespace AdventOfCode.Commons;

/// <summary>
/// Represent the solver used for a given day
/// </summary>
/// 
/// <typeparam name="TInput">
/// The type of the data used to solve the puzzle
/// </typeparam>
/// 
/// <typeparam name="TResult">
/// The type of the result of the puzzle
/// </typeparam>
public abstract class Solver<TInput, TResult>
{
    public readonly Lazy<TInput> Input;

    private Solver(IPuzzleInputReaderStrategy puzzleInputReader)
    {
        Input = new Lazy<TInput>(() =>
        {
            var content = puzzleInputReader.ReadInput();
            return ParseInput(content);
        });
    }

    protected Solver(string inputPath)
        : this(new LocalPuzzleInputReaderStrategy { InputPath = inputPath }) { }

    protected Solver(int year, int day)
        : this(new RemotePuzzleInputReaderStrategy { Year = year, Day = day }) { }


    /// <summary>
    /// Parse the input file and convert it to <typeparamref name="TInput"/>
    /// for it to be used as the input of the <see cref="PartOne(TInput)"/>
    /// and <see cref="PartTwo(TInput)"/>
    /// </summary>
    /// 
    /// <param name="inputPath">
    /// The path of your puzzle input
    /// </param>
    /// 
    /// <returns>
    /// The digested input
    /// </returns>
    public abstract TInput ParseInput(IEnumerable<string> input);

    /// <summary>
    /// Logic for the solution of the first part of the puzzle
    /// </summary>
    /// 
    /// <param name="input">
    /// The digested puzzle input
    /// </param>
    /// 
    /// <returns>
    /// The puzzle's solution
    /// </returns>
    public abstract TResult PartOne(TInput input);

    /// <summary>
    /// Logic for the solution of the second part of the puzzle
    /// </summary>
    /// 
    /// <param name="input">
    /// The digested puzzle input
    /// </param>
    /// 
    /// <returns>
    /// The puzzle's solution
    /// </returns>
    public abstract TResult PartTwo(TInput input);
}
