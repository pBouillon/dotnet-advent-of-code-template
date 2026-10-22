namespace AdventOfCode.Commons;

/// <summary>
/// Settings read from environment variables.
/// </summary>
public static class Configuration
{
    /// <summary>
    /// The name of the environment variable holding an optional contact for the <c>User-Agent</c>.
    /// </summary>
    public const string ContactEnvironmentVariable = "AOC_CONTACT";

    /// <summary>
    /// The name of the environment variable holding the Advent of Code session cookie.
    /// </summary>
    public const string CookieEnvironmentVariable = "AOC_COOKIE";

    /// <summary>
    /// The name of the session cookie expected by the Advent of Code website.
    /// </summary>
    public const string SessionCookieName = "session";

    /// <summary>
    /// An optional contact for the <c>User-Agent</c>, read from <see cref="ContactEnvironmentVariable"/>.
    /// </summary>
    public static string? Contact { get; } = Environment.GetEnvironmentVariable(ContactEnvironmentVariable);

    /// <summary>
    /// The Advent of Code session cookie, read from <see cref="CookieEnvironmentVariable"/>.
    /// </summary>
    public static string? CookieValue { get; } = Environment.GetEnvironmentVariable(CookieEnvironmentVariable);
}
