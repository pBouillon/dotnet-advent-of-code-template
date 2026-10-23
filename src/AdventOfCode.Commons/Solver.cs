using System.Runtime.CompilerServices;

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
    /// <summary>
    /// The puzzle input
    /// </summary>
    public readonly Lazy<TInput> PuzzleInput;

    /// <summary>
    /// Create a new solver and initialize its puzzle input based on the provided <see cref="PuzzleInputReader"/>
    /// </summary>
    /// <param name="puzzleInputReader">The reader to use to retrieve the puzzle input</param>
    private Solver(PuzzleInputReader puzzleInputReader)
    {
        PuzzleInput = new Lazy<TInput>(() => ParseInput(puzzleInputReader.ReadInput()));
    }

    /// <summary>
    /// Create a new solver with a puzzle input located in a local file
    /// </summary>
    /// <param name="inputPath">The path to the file containing the input, relative to the solver's folder</param>
    /// <param name="solverSourcePath">Filled in by the compiler: the solver's source file, used to locate its folder</param>
    protected Solver(string inputPath, [CallerFilePath] string solverSourcePath = "")
        : this(PuzzleInputReader.ForFile(inputPath, solverSourcePath)) { }

    /// <summary>
    /// Create a new solver whose puzzle input is downloaded once, then stored next to the solver
    /// </summary>
    /// <param name="year">The year of the current challenge</param>
    /// <param name="day">The day for which the puzzle input is needed</param>
    /// <param name="solverSourcePath">Filled in by the compiler: the solver's source file, used to locate its folder</param>
    protected Solver(int year, int day, [CallerFilePath] string solverSourcePath = "")
        : this(PuzzleInputReader.ForPuzzle(year, day, solverSourcePath)) { }

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
