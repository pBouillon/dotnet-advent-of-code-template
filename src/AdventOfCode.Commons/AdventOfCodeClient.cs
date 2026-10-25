using System.Net;

namespace AdventOfCode.Commons;

/// <summary>
/// Talks to adventofcode.com: authenticates with the session cookie and identifies the tool.
/// </summary>
internal static class AdventOfCodeClient
{
    /// <summary>
    /// The base address of the Advent of Code website.
    /// </summary>
    private const string BaseUrl = "https://adventofcode.com";

    /// <summary>
    /// The name of the session cookie expected by the Advent of Code website.
    /// </summary>
    private const string SessionCookieName = "session";

    /// <summary>
    /// The shared client, created on first download so users with local inputs never need a cookie.
    /// </summary>
    private static readonly Lazy<HttpClient> Client = new(CreateClient);

    /// <summary>
    /// Downloads the raw input of a puzzle.
    /// </summary>
    /// <exception cref="PuzzleInputUnavailableException">No session cookie is configured.</exception>
    /// <exception cref="HttpRequestException">The server refused the request.</exception>
    public static string DownloadInput(int year, int day)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/{year}/day/{day}/input");
        using var response = Client.Value.Send(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Could not fetch the input of {year} day {day} ({(int)response.StatusCode} {response.ReasonPhrase}). " +
                $"Check that the puzzle is unlocked and that {Configuration.CookieEnvironmentVariable} is valid.");
        }

        using var reader = new StreamReader(response.Content.ReadAsStream());
        return reader.ReadToEnd();
    }

    /// <summary>
    /// Creates the client with the session cookie and the resolved <c>User-Agent</c>.
    /// </summary>
    private static HttpClient CreateClient()
    {
        var handler = new HttpClientHandler
        {
            CookieContainer = GetCookieContainer(),
            UseCookies = true,
        };

        var client = new HttpClient(handler) { BaseAddress = new Uri(BaseUrl) };
        client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgentResolver.Resolve());

        return client;
    }

    /// <summary>
    /// Creates a cookie container holding <see cref="Configuration.CookieValue"/> as the session cookie.
    /// </summary>
    /// <exception cref="PuzzleInputUnavailableException">The cookie is not set.</exception>
    private static CookieContainer GetCookieContainer()
    {
        var cookie = Configuration.CookieValue?.Trim();

        if (string.IsNullOrEmpty(cookie))
        {
            throw new PuzzleInputUnavailableException(
                $"{Configuration.CookieEnvironmentVariable} is not set, neither as an environment variable nor in a {DotEnv.FileName} file. "
                + "Set it to your Advent of Code session cookie to fetch your puzzle input.");
        }

        var container = new CookieContainer();
        container.Add(new Cookie(SessionCookieName, cookie, "/", ".adventofcode.com"));

        return container;
    }
}
