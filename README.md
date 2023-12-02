# Dotnet Advent of Code

[Advent of Code](https://adventofcode.com) template for .NET contenders, focus on the puzzle, let it take care the rest

> After gaining yours, consider leaving a small ⭐ to this project too!

---

## Advantages

In order to solve your puzzle, you might need the debugger from time to time, especially as the puzzles become harder.

This template is built around test projects, in order to take advatage of the debugging possibilities and TDD if you want to.

Here are also a bunch of features offered by the template:

- Retrieval of the puzzle input localy or remotely
- Testing of your modelization if you would like to
- Testing of the puzzle examples
- Conditionnaly skip the tests of a puzzle if wanted

## Usage

This template exposes two classes: a `Solver` and a `TestEngine`.
The [.NET CI](.github/workflows/dotnet.yml) is also preconfigured to ensure that all your tests are valid.

To get started with a new year, just create a new `xUnit` project and add a reference to the `AdventOfCode.Commons` project to it.

> If you want to jump straight to the code for the usage, check the [demo project](src/AdventOfCode.Usage)

### Solving a new puzzle

When working a new day, create a new `Solver` by inheriting the `Solver<TInput, TResult>` class,
with `TInput` being the type of the input you will be working with and `TResult` the type of the result you are expecting.

You will then have to implement three different logics:

1. **The parsing of the input** in which you will have to convert the raw data from the file you are reading into the `TInput`
  you will be expecting afterwards
2. **The logic for the first part of the puzzle**
3. **The logic for the second part of the puzzle**

For example, if the first part of the puzzle is "Given a list of integers, find the greatest one" and the second one "Now find the sum of them", we can do the following:

```csharp
public class Solver : Solver<int[], int>
{
    public Solver() : base(inputPath: "WithLocalInput/input.txt") { }

    public override int PartOne(int[] input)
        => input.Max();

    public override int PartTwo(int[] input)
        => input.Sum();

    public override int[] ParseInput(IEnumerable<string> input)
        => input.Select(int.Parse).ToArray();
}
```

If you prefer fetching your input from the server, you can instead specify the date:

```csharp
public class Solver : Solver<int[], int>
{
    public Solver() : base(year: 0000, day: 00) { }

    public override int PartOne(int[] input)
        => input.Max();

    public override int PartTwo(int[] input)
        => input.Sum();

    public override int[] ParseInput(IEnumerable<string> input)
        => input.Select(int.Parse).ToArray();
}
```

### Testing your solution

Once that your have coded your `Solver`, you may want to test it.

To do so, create a new class inheriting from `TestEngine<TSolver, TInput, TResult>`, with `TSolver` the type of your solver and
`TInput` and `TResult` the same as it.

You will then have to implement two `Puzzle` properties.
Those represent the input of each part of the puzzle of the day.

For each part you will have to specify what the example is (its input and solution) and the solution you are expecting.
Specifying the example allows the engine to test your solution against a predictible result in order to help you to debug it.

Keeping our example in mind, the associated `TestEngine` might be:

```csharp
public class SolverTest : TestEngine<Solver, int[], int>
{
    public override Puzzle PartOne => new()
    {
        ShouldSkipTests = false,  // Default to false
        Example = new()
        {
            Input = new[] { 1, 2, 3 },
            Result = 3,
        },
        Solution = 5,
    };

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

### Testing your parsing

This testing feature allows you to verify the correctness of the `ParseInput` method by
comparing its output with a manually provided `RawInput`. It's completely optional, so
you can choose to use it based on your preferences.

If you would like to, define the `RawInput` property along with your `Input` in your `Example`.  
If the `RawInput` is not set or empty, this part won't be tested.

```cs
public override Puzzle PartOne => new()
{
    Example = new()
    {
        // 👇 Since this is defined, a test will run to check if `ParseInput(RawInput)` is equal to `Input`
        RawInput = ["1", "2", "3"],
        Input = [1, 2, 3],
        Result = 3,
    },
    Solution = 5,
};
```

> Since the example might change from one part to the other, the `RawInput` is defined in each part instead of on the puzzle-level

## Troubleshooting

### How can I find my cookie?

Under the website of the [Advent of Code](https://adventofcode.com), open the dev tools and find the Advent of Code cookie in your storage:

![Example](https://user-images.githubusercontent.com/22640284/205501479-31e2e5ef-d50e-43f8-8a45-4741a473861c.png)

You can then copy its value.

### Where can I set my cookie?

You can either provide the cookie as an environment variable named `AOC_COOKIE`, or directly set it in the `Configuration.cs` file:

```csharp
public static class Configuration
{
    public readonly static string? CookieValue = "YOUR-COOKIE-HERE";
}
```

### My input is missing!

Be sure to have your input data accessible, you can do so by indicating how VS should take care of your file:

![image](https://user-images.githubusercontent.com/22640284/205364254-5e1b7995-d267-4809-8ffa-5e68efe84b84.png)

### No tests are showing up!

Verify if your `TestEngine` implementation is `public` or `xUnit` might have some trouble finding it.
