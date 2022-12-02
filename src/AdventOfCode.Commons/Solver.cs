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
    /// The path to the input file (ex: <c>"day1/input.txt"</c>)
    /// </summary>
    protected abstract string InputPath { get; }

    public readonly TInput Input;

    protected Solver()
        => Input = ReadInput(InputPath);

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
    public abstract TInput ReadInput(string inputPath);
}
