using System.Net;

namespace AdventOfCode.Commons.PuzzleInputReaderStrategy;

/// <summary>
/// Strategy to retrieve the input from the advent of code server
/// </summary>
public class RemotePuzzleInputReaderStrategy : IPuzzleInputReaderStrategy
{
    private const string AdventOfCodeBaseUrl = "https://adventofcode.com";

    /// <summary>
    /// The year targeted by this reader
    /// </summary>
    public required int Year { get; init; }

    /// <summary>
    /// The day of the puzzle whose entry we want to retrieve
    /// </summary>
    public required int Day { get; init; }

    /// <summary>
    /// The inner HTTP client handler, preconfigured to use cookies and provide the value set in
    /// <see cref="Configuration.CookieValue"/>
    /// </summary>
    private static HttpClientHandler Handler => new()
    {
        CookieContainer = GetCookieContainer(),
        UseCookies = true,
    };

    /// <summary>
    /// The inner HTTP client, preconfigured to make requests to the advent of code URI
    /// </summary>
    private static HttpClient Client => new(Handler)
    {
        BaseAddress = new Uri(AdventOfCodeBaseUrl),
    };

    /// <summary>
    /// Create a new cookie container with the one set in <see cref="Configuration.CookieValue"/>
    /// </summary>
    /// 
    /// <returns>
    /// The cookie container for the <see cref="HttpClientHandler"/> to use
    /// </returns>
    /// 
    /// <exception cref="Exception">
    /// Thrown if it is called but <see cref="Configuration.CookieValue"/> is not set
    /// </exception>
    private static CookieContainer GetCookieContainer()
    {
        var container = new CookieContainer();

        container.Add(new Cookie
        {
            Name = "session",
            Domain = ".adventofcode.com",
            Value = string.IsNullOrEmpty(Configuration.CookieValue)
                ? throw new Exception("You need to specify your cookie in order to get your input puzzle")
                : Configuration.CookieValue,
        });

        return container;
    }

    /// <inheritdoc />
    public IEnumerable<string> ReadInput()
    {
        var task = Task.Run(async () =>
        {
            var response = await Client.GetAsync($"/{Year}/day/{Day}/input");

            return await response.EnsureSuccessStatusCode()
                .Content
                .ReadAsStringAsync();
        });

        task.Wait();

        var content = task.Result;
        return content.Split("\n")[..^1];
    }
}
