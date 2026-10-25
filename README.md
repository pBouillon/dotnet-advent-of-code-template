# .NET Advent of Code Template

A small template for solving [Advent of Code][aoc] puzzles in C#. Each puzzle
is a test class: you write the solver, describe the example from the puzzle
statement, and run it from your IDE.

## What it does for you

- **Checks each step separately.** Parsing, the puzzle's example and your real
  answer are three tests per part, so a failure tells you which step is wrong.
- **Fetches your input once.** It is downloaded with your session cookie,
  saved next to your solver and never requested again. You can also point to a
  file you saved yourself.
- **Skips what isn't ready.** A part you haven't started, or an input you
  don't have: those tests are skipped and the others still run.
- **Works with your IDE.** It is a regular xUnit v3 project: set breakpoints,
  run a single test, re-run on save.

> If this template helps you earn your stars, consider leaving one on this
> repository too! ⭐

## Getting started

**Requirements:** the [.NET 10 SDK][dotnet].

1. Create an **xUnit v3** project (one per year works well) and reference the
   `AdventOfCode.Commons` project.
2. Add a solver and its test, as below.
3. [Set your session cookie](#setting-your-cookie), or
   [use a file](#getting-your-input) instead.
4. Run `dotnet test`, or run the tests from your IDE.

`AdventOfCode.Commons` is plain source in your repository, so you can change
anything in it. A complete example lives in the [demo project][demo].

### The solver

Inherit `Solver<TInput, TResult>`, where `TInput` is the shape you parse the
input into and `TResult` the type of your answers, then implement
`ParseInput`, `PartOne` and `PartTwo`:

```csharp
public sealed class Solver()
    : Solver<int[], int>(year: 2021, day: 1)
{
    public override int[] ParseInput(IEnumerable<string> input)
        => [.. input.Select(int.Parse)];

    public override int PartOne(int[] input)
    {
        var count = 0;

        for (var i = 1; i < input.Length; ++i)
        {
            if (input[i - 1] < input[i]) ++count;
        }

        return count;
    }

    public override int PartTwo(int[] input)
    {
        var windowSums = Enumerable.Range(0, input.Length - 2)
            .Select(offset => input[offset..(offset + 3)].Sum())
            .ToArray();

        return PartOne(windowSums);
    }
}
```

### The test

Inherit `TestEngine<TSolver, TInput, TResult>` and describe each part:

```csharp
public sealed class SolverTest : TestEngine<Solver, int[], int>
{
    public override Puzzle PartOne => PuzzleBuilder
        // The example's raw lines, as written in the puzzle statement
        .FromInput([
            "199", "200", "208", "210", "200",
            "207", "240", "269", "260", "263",
        ])
        // What ParseInput should return for them
        .ParsedAs([199, 200, 208, 210, 200, 207, 240, 269, 260, 263])
        // The example's expected answer
        .ExpectsResult(7)
        // The answer for your own input, once Advent of Code accepted it
        .WithTheActualSolutionBeing(0);

    public override Puzzle PartTwo => PuzzleBuilder
        // Gives the parsed example directly, which skips the parsing test
        .FromParsedInput([199, 200, 208, 210, 200, 207, 240, 269, 260, 263])
        .ExpectsResult(5)
        .WithTheActualSolutionBeing(0);
}
```

Fill in `WithTheActualSolutionBeing` once Advent of Code accepts your answer.
Until then that one test fails, while the Parsing and Example tests already
tell you whether you are on the right track.

You can also create the `Puzzle` directly, with an `Example` (`RawInput`,
`Input`, `Result`) and a `Solution`. `RawInput` enables the parsing test.

## Getting your input

Your solver's constructor decides where its input comes from:

- **`year` and `day`**: the input is downloaded the first time a solution test
  needs it, saved next to the solver as `input.<year>-day<DD>.txt` (for
  example `input.2021-day01.txt`), and read from that file from then on.
- **`inputPath`**: the input is read from a file you saved yourself, relative
  to the solver's folder. No cookie and no network request involved.

Parsing and example tests never need the input. When it isn't available (no
file, and no cookie to download it), only the solution tests are skipped, with
a message telling you why. If a download was attempted and failed, the test
fails instead.

### Setting your cookie

Downloads need your Advent of Code session cookie (see the [FAQ](#faq) to find
it). Store it in an environment variable named `AOC_COOKIE`:

**Bash / zsh**

```bash
# Keep it for every future terminal (use ~/.zshrc if you are on zsh)
echo 'export AOC_COOKIE="your-cookie-value"' >> ~/.bashrc

# Or only for the current terminal
export AOC_COOKIE="your-cookie-value"
```

**PowerShell**

```powershell
# Keep it for your user account, for every future terminal
setx AOC_COOKIE "your-cookie-value"

# Or only for the current terminal
$env:AOC_COOKIE = "your-cookie-value"
```

Restart your IDE and terminal afterwards: they only read environment variables
when they start.

Prefer a file? Put it in a `.env` file, see [Configuration](#configuration).

> The cookie is as good as being logged in, so treat it like a password. The
> template's `.gitignore` ignores `.env`, and also your inputs (`input*.txt`),
> since Advent of Code asks people not to share them.

## Configuration

Two settings are available. Each one can be an environment variable or an entry
in a `.env` file, and the environment variable wins when both are set.

| Setting       | Needed        | What it does                            |
| ------------- | ------------- | --------------------------------------- |
| `AOC_COOKIE`  | For downloads | Your Advent of Code session cookie      |
| `AOC_CONTACT` | No            | Contact sent in the `User-Agent` header |

`AOC_CONTACT` is how Advent of Code can reach you if your requests ever cause
trouble: an email, a name or a URL all work. When it isn't set, the template
uses your git `user.email`, then your git `user.name`, then your machine's
hostname. Accents and other non-ASCII characters are dropped, as HTTP headers
only allow ASCII.

Example `.env`:

```dotenv
# Advent of Code
AOC_COOKIE=your-cookie-value
AOC_CONTACT=you@example.com
```

Put it at the root of your repository (any folder above your project works
too) and keep it out of git: the template's `.gitignore` already ignores it.
The nearest file that defines a setting wins. The format is:

- one `KEY=VALUE` per line;
- `#` starts a comment, on its own line;
- an `export ` prefix is accepted;
- values can be wrapped in single or double quotes.

Variable expansion and multi-line values aren't supported.

## Skipping tests

Stuck on a part? Skip all of its tests with `ShouldSkipTests = true` on the
`Puzzle`, or `skipTests: true` at the end of the builder:

```csharp
.WithTheActualSolutionBeing(5, skipTests: true)
```

## FAQ

<details>
<summary>
Why isn't this a NuGet package?
</summary>

The template needs your personal session cookie to fetch inputs. Keeping
`AdventOfCode.Commons` as a project reference inside your repository means you
stay in control of how that cookie is managed, so nothing is unintentionally
included or exposed through a shared package. It also lets you change the
library however you like.

</details>

<details>
<summary>
How can I find my AoC session cookie?
</summary>

Log in to [adventofcode.com][aoc], open your browser's developer tools
(usually `F12`) and look in the Application or Storage tab for the `session`
cookie:

[![Example][cookie-image]][cookie-image]

Then copy its value.

</details>

<details>
<summary>
Where can I set my cookie?
</summary>

In the `AOC_COOKIE` environment variable or in a `.env` file, see
[Setting your cookie](#setting-your-cookie).

</details>

<details>
<summary>
Where are my inputs stored?
</summary>

- Downloaded inputs are saved next to the solver that uses them, as
  `input.<year>-day<DD>.txt`.
- Inputs read from a file are wherever you pointed `inputPath` to, relative to
  the solver's folder.

Both are ignored by git, as long as they are named `input*.txt`. To download
an input again, delete its file and run your tests.

</details>

<details>
<summary>
My input is missing!
</summary>

Your solution tests are skipped, and the message says why. With "No puzzle
input found", the file isn't where your solver looks: check that it exists and
that the path is relative to the solver's folder. With "AOC_COOKIE is not
set", the input has to be downloaded: set your cookie, or save the input
yourself and use `inputPath`.

</details>

<details>
<summary>
My input can't be downloaded!
</summary>

The error includes the HTTP status. Check that the puzzle is unlocked (inputs
are released at midnight EST) and that your cookie hasn't expired: log in to
Advent of Code again to get a fresh one.

</details>

<details>
<summary>
Does this respect the Advent of Code automation guidelines?
</summary>

Yes! The template follows the [automation guidelines][guidelines]:

- Inputs are cached: once downloaded, they are read from disk and never
  requested again.
- Downloads are made one at a time, and only when a test needs an input.
- Requests carry a `User-Agent` naming this template and a contact:
  `AOC_CONTACT` if you set it (environment variable or `.env`), otherwise your
  git `user.email`, then your git `user.name`, then your machine's hostname.

Set `AOC_CONTACT` if you'd rather not send your git email.

</details>

<details>
<summary>
No tests are showing up!
</summary>

Make sure your test class is `public`. xUnit v3 runs on Microsoft Testing
Platform, so keep your IDE up to date (Visual Studio 17.14 or later).

</details>

[aoc]: https://adventofcode.com
[dotnet]: https://dotnet.microsoft.com/download
[demo]: https://github.com/pBouillon/dotnet-advent-of-code-template/blob/main/src/AdventOfCode.Usage
[guidelines]: https://www.reddit.com/r/adventofcode/wiki/faqs/automation
[cookie-image]: https://user-images.githubusercontent.com/22640284/205501479-31e2e5ef-d50e-43f8-8a45-4741a473861c.png
