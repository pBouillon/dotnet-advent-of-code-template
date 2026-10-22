# 🎄 .NET Advent of Code Template

**A ready-to-go Advent of Code template for .NET contenders.** **Focus on solving the puzzle; we handle the boilerplate, input, and testing.**

This template's core strength is its foundation in **xUnit v3 test projects**, which provides two massive advantages for complex AoC puzzles: **seamless debugging** and native support for **Test-Driven Development (TDD)**.

Here are the features offered by the template:

- **Automatic Input Retrieval** (local file, or fetched once from the website and kept in your project)
- **Modeling/Parsing Tests** to allow you to model the problem as you want
- **Example Tests** using provided examples and expected results to ensure your solution satisfies it
- **Conditional Test Skipping** focus only on the parts you're currently working on, and let missing inputs skip themselves
- Pre-configured [**.NET CI**](https://github.com/pBouillon/dotnet-advent-of-code-template/blob/main/.github/workflows/dotnet.yml) running your tests on every push to `main` and every pull request

> If this template helps you earn your stars, consider leaving one on this repository too! ⭐

## Usage & Setup

This template provides two core components: the **`Solver`** (for logic) and the **`TestEngine`** (for verification).

**Prerequisites:** the [.NET 10 SDK](https://dotnet.microsoft.com/download).

**Setup:** To begin a new year, simply create a new **`xUnit v3`** project and add a reference to the **`AdventOfCode.Commons`** library.

> If you want to jump straight to the code, check out the [demo project](https://github.com/pBouillon/dotnet-advent-of-code-template/blob/main/src/AdventOfCode.Usage).

### Solving a New Puzzle

Create a new solver by inheriting the generic `Solver<TInput, TResult>` class, where `TInput` is your parsed data model and `TResult` is the expected result type.

Your `Solver` requires you to implement three core methods:

1. **`ParseInput`** : Convert raw `IEnumerable<string>` input lines into your strongly-typed `TInput` model.
2. **`PartOne`** : Implement the logic for the first part of the puzzle.
3. **`PartTwo`** : Implement the logic for the second part of the puzzle.

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

### Getting Your Input

Your solver gets its input one of two ways, depending on the constructor you call:

- **From a file**: pass the path of your input, relative to your project's folder (the one holding the `.csproj`).
- **From the website**: pass the year and the day. The first time it's needed, the input is downloaded with your session cookie and stored in `Inputs/<year>/day<DD>.txt` in your project. From then on, it's read from there: it is never fetched twice.

Either way, the input is only read when a **solution** test needs it. Parsing and example tests run without it.

> Advent of Code asks not to share puzzle inputs, so keep them out of your repository: the template's `.gitignore` already ignores the `Inputs/` folder and `input*.txt` files.

### Testing with the Fluent API

Once you have implemented your `Solver`, create your test class by inheriting from `TestEngine<TSolver, TInput, TResult>`.

You will define the expected inputs and solutions via the `PartOne` and `PartTwo` properties.

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

If you are stuck on a puzzle part, you can conditionally **skip all tests** for that part by using the builder, or setting `ShouldSkipTests = true` on the `Puzzle` definition:

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

Alternatively, you can **skip only the parsing step** (if you prefer to define the input as `TInput` directly) by using `PuzzleBuilder.FromParsedInput` or leaving `Example.RawInput` empty:

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

#### Missing inputs

No flag needed here. When the **solution** test of a part can't find its input (no file at the given path, or no `AOC_COOKIE` to download it), it is skipped with a message telling you why. Your parsing and example tests still run, which is what keeps your CI green without your inputs.

If the input *was* reachable but the download failed (expired cookie, puzzle not unlocked yet, no network...), the test fails instead, so a real problem never hides behind a skip.

## Troubleshooting & FAQ

**Why isn't this a NuGet package?**

This project is primarily intended to be used as a local template or library reference rather than a published NuGet package. The main reason for this is related to input retrieval from the official Advent of Code site.

To automatically fetch your puzzle input remotely, the template requires your personal AoC session cookie.

- If this library were published on NuGet, distributing it would require either hardcoding the input retrieval logic that depends on a local file or environment variable, or potentially handling cookie storage in a way that might raise security/privacy concerns for some users.
- By keeping the common components as an `AdventOfCode.Commons` project reference within your repository, you retain full control over how your session cookie is managed, preventing any information from being unintentionally included or exposed.

**How can I find my AoC session cookie?**

Under the website of the [Advent of Code](https://adventofcode.com/), open the developer tools (usually `F12`) and inspect the application/storage tab to find the session cookie:

[![Example](https://user-images.githubusercontent.com/22640284/205501479-31e2e5ef-d50e-43f8-8a45-4741a473861c.png)](https://user-images.githubusercontent.com/22640284/205501479-31e2e5ef-d50e-43f8-8a45-4741a473861c.png)

You can then copy its value.

**Where can I set my cookie?**

Set it as an environment variable named `AOC_COOKIE`:

- **PowerShell:** `setx AOC_COOKIE "your-cookie-value"`
- **Bash / zsh:** `export AOC_COOKIE="your-cookie-value"` (add it to your shell profile to keep it)

Then restart your IDE or terminal: they only read environment variables when they start.

The cookie is only needed when an input has to be downloaded, and the template never writes it anywhere, so there is nothing to accidentally commit.

**Where are my inputs stored?**

- Inputs fetched from the website are stored in `Inputs/<year>/day<DD>.txt`, in your project's folder (next to the `.csproj`).
- Inputs read from a file are wherever you pointed `inputPath` to, relative to that same folder.

Both are ignored by git. To fetch an input again (say it got corrupted), delete its file and run your tests.

**My input is missing!**

Your solution tests are skipped with a "No puzzle input found" message. For a local input, check that the file exists and that the path given to your solver is relative to your project's folder. For a remote input, set your cookie, see above.

**My input can't be downloaded!**

The error includes the HTTP status. Make sure the puzzle is unlocked (inputs are available at midnight EST) and that your cookie hasn't expired: log in to Advent of Code again to get a fresh one.

**Does this respect the Advent of Code automation guidelines?**

Yes! The template follows the [automation guidelines](https://www.reddit.com/r/adventofcode/wiki/faqs/automation) of the /r/adventofcode community wiki:

- Inputs are cached: once downloaded, they are read from your project and never fetched again.
- Downloads are made one at a time, and only when a test needs an input.
- Requests identify the template and give a contact in their `User-Agent`: the value of `AOC_CONTACT` if you set it, otherwise your git `user.email`, then your git `user.name`, then your machine's hostname.

If you'd rather not send your git email, set `AOC_CONTACT` to whatever you want to be reached at.

**No tests are showing up!**

Verify if your `TestEngine` implementation is `public` or `xUnit` might have some trouble finding it. Since xUnit v3 runs on the Microsoft Testing Platform, also make sure your Visual Studio is up to date (17.14 or later).
