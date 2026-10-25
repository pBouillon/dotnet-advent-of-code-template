namespace AdventOfCode.Commons;

/// <summary>
/// Settings read from environment variables, falling back to a <c>.env</c> file.
/// </summary>
public static class Configuration
{
    /// <summary>The name of the setting holding the Advent of Code session cookie.</summary>
    public const string CookieEnvironmentVariable = "AOC_COOKIE";

    /// <summary>
    /// The name of the setting holding an optional contact for the <c>User-Agent</c>.
    /// </summary>
    public const string ContactEnvironmentVariable = "AOC_CONTACT";

    /// <summary>
    /// The Advent of Code session cookie, read from <see cref="CookieEnvironmentVariable"/>.
    /// </summary>
    public static string? CookieValue { get; } = Resolve(CookieEnvironmentVariable);

    /// <summary>
    /// An optional contact for the <c>User-Agent</c>, read from <see cref="ContactEnvironmentVariable"/>.
    /// </summary>
    public static string? Contact { get; } = Resolve(ContactEnvironmentVariable);

    /// <summary>
    /// Returns the value of the environment variable <paramref name="name"/>, or else its value in a <c>.env</c> file.
    /// </summary>
    private static string? Resolve(string name)
    {
        var value = Environment.GetEnvironmentVariable(name);

        return string.IsNullOrWhiteSpace(value) ? DotEnv.Get(name) : value;
    }
}
