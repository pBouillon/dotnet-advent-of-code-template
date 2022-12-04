namespace AdventOfCode.Commons;

/// <summary>
/// Configure the constants and values required for the application to run
/// </summary>
public static class Configuration
{
    /// <summary>
    /// Your session cookie, if you want to retrieve your input from the internet
    /// </summary>
    ///
    /// <remarks>
    /// To get your cookie, head to https://adventofcode.com/, then look into the cookies
    /// the website has set and paste its value here, it be something like <c>session:"[value]"</c>
    /// </remarks>
    public readonly static string? CookieValue = Environment.GetEnvironmentVariable("AOC_COOKIE");
}
