# Dotnet Advent of Code

[Advent of Code](https://adventofcode.com) template for .NET contenders, focus on the puzzle, let it handle the rest 

> After gaining yours, consider leaving a small ⭐ to this project too!

---

## Usage

This template exposes two classes: a `Solver` and a `TestEngine`.
The [.NET CI](.github/workflows/dotnet.yml) is also preconfigured to ensure that all your tests are valid.

To get started with a new year, just create a new xUnit project and add a reference to the `AdventOfCode.Commons` project to it.

> If you want to jump straight to the code for the usage, check the [demo project](src/AdventOfCode.Usage)

### Solving a new puzzle

When working a new day, create a new `Solver` by inheriting the `Solver<TInput, TResult>` class,
with `TInput` being the type of the input you will be working with and `TResult` the type of the result you are expecting.

You will then have to implement three different logics:

1. **The parsing of the input** in which you will have to convert the raw data from the file you are reading into the `TInput`
  you will be expecting afterwards
2. **The logic for the first part of the puzzle**
3. **The logic for the second part of the puzzle**

> For example, if the first part of the puzzle is "Given a list of integers, find the greatest one" and the second one 
> "Now find the sum of them", we can do the following:
> 
> ```csharp
> public class Solver : Solver<int[], int>
> {
>     protected override string InputPath => "input.txt";
> 
>     public override int PartOne(int[] input)
>         => input.Max();
> 
>     public override int PartTwo(int[] input)
>         => input.Sum();
> 
>     public override int[] ReadInput(string inputPath)
>         => File
>             .ReadAllLines(inputPath)
>             .Select(int.Parse)
>             .ToArray();
> }
> ```

### Testing your solution

Once that your have coded your `Solver`, you may want to test it.

To do so, create a new class inheriting from `TestEngine<TSolver, TInput, TResult>`, with `TSolver` the type of your solver and
`TInput` and `TResult` the same as it.

You will then have to implement two `Puzzle` properties.
Those represent the input of each part of the puzzle of the day.

For each part you will have to specify what the example is (its input and solution) and the solution you are expecting.
Specifying the example allows the engine to test your solution against a predictible result in order to help you to debug it.

> Keeping our example in mind, the associated `TestEngine` might be:
> 
> ```csharp
> public class SolverTest : TestEngine<Solver, int[], int>
> {
>     public override Puzzle PartOne => new()
>     {
>         Example = new()
>         {
>             Input = new[] { 1, 2, 3 },
>             Result = 3,
>         },
> 
>         Solution = 5,
>     };
> 
>     public override Puzzle PartTwo => new()
>     {
>         Example = new()
>         {
>             Input = new[] { 1, 2, 3 },
>             Result = 6,
>         },
> 
>         Solution = 15,
>     };
> }
> ```

## Troubleshooting

### My input is missing!

Be sure to have your input data accessible, you can do so by indicating how VS should take care of your file:

![image](https://user-images.githubusercontent.com/22640284/205364254-5e1b7995-d267-4809-8ffa-5e68efe84b84.png)

### No tests are showing up!

Verify if your `TestEngine` implementation is `public` or `xUnit` might have some trouble finding it.
