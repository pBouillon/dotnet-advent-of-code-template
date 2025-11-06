# 🎄 .NET Advent of Code Template

**A ready-to-go **Advent of Code** template for .NET contenders.**
**Focus on solving the puzzle; we handle the boilerplate, input, and testing.**

This template's core strength is its foundation in **xUnit test projects**, which
provides two massive advantages for complex AoC puzzles: **seamless debugging** and native support for **Test-Driven Development (TDD)**.

Here are the features offered by the template:

* **Automatic Input Retrieval** (local file or remotely fetches your input for a given day)
* **Modeling/Parsing Tests** to allow you to model the problem as you want
* **Example Tests** using provided examples and expected results to ensure your solution satisfies it
* **Conditional Test Skipping** focus only on the parts you're currently working on
* Pre-configured **[.NET CI](.github/workflows/dotnet.yml)** to ensure all puzzles are valid

> If this template helps you earn your stars, consider leaving one on this repository
> too! ⭐

## Usage & Setup

This template provides two core components: the **`Solver`** (for logic) and the
**`TestEngine`** (for verification).

**Setup:** To begin a new year, simply create a new **`xUnit`** project and add a
reference to the **`AdventOfCode.Commons`** library.

> If you want to jump straight to the code, check out the
> [demo project](src/AdventOfCode.Usage).

### Solving a New Puzzle

Create a new solver by inheriting the generic `Solver<TInput, TResult>` class,
where `TInput` is your parsed data model and `TResult` is the expected result type.

Your `Solver` requires you to implement three core methods:

1.  **`ParseInput`**: Convert raw `IEnumerable<string>` input lines into your strongly-typed `TInput` model.
2.  **`PartOne`**: Implement the logic for the first part of the puzzle.
3.  **`PartTwo`**: Implement the logic for the second part of the puzzle.

**Example Solver (Local Input):**

```csharp
public class Solver()
    : Solver<int[], int>(inputPath: "WithLocalInput/input.txt")
{
    public override int PartOne(int[] input)
        => input.Max();

    public override int PartTwo(int[] input)
        => input.Sum();

    public override int[] ParseInput(IEnumerable<string> input)
        => input.Select(int.Parse).ToArray();
}
```

**Example Solver (Remote Input):**

```csharp
public class Solver()
    : Solver<int[], int>(year: 2025, day: 01) { ... }
```

### Testing with the Fluent API

Once you have implemented your `Solver`, create your test class by inheriting from
`TestEngine<TSolver, TInput, TResult>`.

You will define the expected inputs and solutions via the `PartOne` and `PartTwo`
properties.

```csharp
public sealed class SolverTest : TestEngine<Solver, int[], int>
{
    // Define Part One using the fluent builder
    public override Puzzle PartOne => PuzzleBuilder
        // 1. The raw input lines for the example
        .FromInput(["1", "2", "3"])
        // 2. The expected result of the ParseInput method (TInput)
        .ParsedAs([1, 2, 3])
        // 3. The expected result for the example input
        .ExpectsResult(3)
        // 4. The correct answer for the main puzzle input
        .WithTheActualSolutionBeing(5);

    // You can also directly create the Puzzle object
    public override Puzzle PartTwo => new()
    {
        Example = new()
        {
            Input = new[] { 1, 2, 3 },
            Result = 6,
        },
        Solution = 15,
    };
}
```

### Skipping Tests

If you are stuck on a puzzle part, you can conditionally **skip all tests** for
that part by using the builder, or setting `ShouldSkipTests = true` on the `Puzzle`
definition:

```csharp
public sealed class SolverTest
    : TestEngine<Solver, int[], int>
{
    public override Puzzle PartOne => PuzzleBuilder
        // ...
        .WithTheActualSolutionBeing(5, skipTests: true); // Builder shortcut

    public override Puzzle PartTwo => new()
    {
        // ...
        ShouldSkipTests = true, // Direct property
    };
}
```

Alternatively, you can **skip only the parsing step** (if you prefer to define the
input as `TInput` directly) by using `PuzzleBuilder.FromParsedInput` or leaving
`Example.RawInput` empty:

```csharp
public sealed class SolverTest
    : TestEngine<Solver, int[], int>
{
    public override Puzzle PartOne => PuzzleBuilder
        .FromParsedInput([1, 2, 3])
        .ExpectsResult(3)
        .WithTheActualSolutionBeing(5);

    // ...
}
```

## Troubleshooting & FAQ

<details>
<summary><b>Why isn't this a NuGet package?</b></summary>

This project is primarily intended to be used as a local template or library reference
rather than a published NuGet package. The main reason for this is related to input
retrieval from the official Advent of Code site.

To automatically fetch your puzzle input remotely, the template requires your personal
AoC session cookie.

- If this library were published on NuGet, distributing it would require either
  hardcoding the input retrieval logic that depends on a local file or environment
  variable, or potentially handling cookie storage in a way that might raise
  security/privacy concerns for some users.
- By keeping the common components as an `AdventOfCode.Commons` project reference
  within your repository, you retain full control over how your session cookie is
  managed, preventing any information from being unintentionally included or
  exposed.

</details>

<details>
<summary><b>How can I find my AoC session cookie?</b></summary>

Under the website of the [Advent of Code](https://adventofcode.com/), open the
developer tools (usually `F12`) and inspect the application/storage tab to find
the session cookie:

![Example](https://user-images.githubusercontent.com/22640284/205501479-31e2e5ef-d50e-43f8-8a45-4741a473861c.png)

You can then copy its value.

</details>

<details>
<summary><b>Where can I set my cookie?</b></summary>

You can either provide the cookie as an environment variable named `AOC_COOKIE`, or
directly set it in the `Configuration.cs` file:

```csharp
public static class Configuration
{
    public readonly static string? CookieValue = "YOUR-COOKIE-HERE";
}
```

</details>

<details>
<summary><b>My input is missing!</b></summary>

Be sure to have your input data accessible, you can do so by indicating how VS should
take care of your file:

![image](https://user-images.githubusercontent.com/22640284/205364254-5e1b7995-d267-4809-8ffa-5e68efe84b84.png)

</details>

<details>
<summary><b>No tests are showing up!</b></summary>

Verify if your `TestEngine` implementation is `public` or `xUnit` might have some
trouble finding it.

</details>