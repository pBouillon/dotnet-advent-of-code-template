namespace AdventOfCode.Commons;

/// <summary>
/// Thrown when a puzzle input is neither stored locally nor retrievable, so dependent tests can be skipped.
/// </summary>
public sealed class PuzzleInputUnavailableException(string message)
: Exception(message);
