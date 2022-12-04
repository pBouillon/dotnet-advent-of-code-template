using System.Net;

namespace AdventOfCode.Commons.Input;

public class RemotePuzzleInputReaderStrategy : IPuzzleInputReaderStrategy
{
    private const string AdventOfCodeBaseUrl = "https://adventofcode.com";
    
    public const string Cookie = "53616c7465645f5f24418ae85f2ee16ffe226239823eded5e30b5f5dee7817447b92dd8ac6207ed1838b8ef68429c61bac187a1e2b7e7cd1e747ea82fe417a96";

    public required int Year { get; init; }
    
    public required int Day { get; init; }

    private static HttpClientHandler Handler => new()
    {
        CookieContainer = GetCookieContainer(),
        UseCookies = true,
    };

    private static HttpClient Client => new(Handler)
    {
        BaseAddress = new Uri(AdventOfCodeBaseUrl),
    };

    private static CookieContainer GetCookieContainer()
    {
        var container = new CookieContainer();

        container.Add(new Cookie
        {
            Name = "session",
            Domain = ".adventofcode.com",
            Value = string.IsNullOrEmpty(Cookie)
                ? throw new Exception("You need to specify your cookie in order to get your input puzzle")
                : Cookie,
        });

        return container;
    }

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
