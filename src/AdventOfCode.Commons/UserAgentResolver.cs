using System.Text;

namespace AdventOfCode.Commons;

/// <summary>
/// Builds the <c>User-Agent</c> sent to adventofcode.com: the tool name followed by a contact.
/// The result is resolved once and reused for the lifetime of the process.
/// </summary>
internal static class UserAgentResolver
{
    /// <summary>
    /// Identifies this template and links to its repository.
    /// </summary>
    private const string ToolName =
        "dotnet-advent-of-code-template (+https://github.com/pBouillon/dotnet-advent-of-code-template)";

    /// <summary>
    /// Contact sources by priority, highest first; a source returns <c>null</c> when it has nothing.
    /// </summary>
    private static readonly IReadOnlyList<Func<string?>> DefaultContactSources =
    [
        () => Configuration.Contact,
        () => GitConfig.Get("user.email"),
        () => GitConfig.Get("user.name"),
        () => Environment.MachineName,
    ];

    /// <summary>
    /// Holds the resolved value so the contact sources are queried at most once.
    /// </summary>
    private static readonly Lazy<string> Cached = new(() => Resolve(DefaultContactSources));

    /// <summary>
    /// Returns the <c>User-Agent</c>, resolving it on first use.
    /// </summary>
    public static string Resolve()
        => Cached.Value;

    /// <summary>
    /// Builds a <c>User-Agent</c> from the first usable source, using "unknown" if none is.
    /// Bypasses the cache, which lets tests supply their own sources.
    /// </summary>
    internal static string Resolve(IEnumerable<Func<string?>> contactSources)
    {
        var contact = contactSources
            .Select(Attempt)
            .FirstOrDefault(value => value is not null);

        return $"{ToolName} by {contact ?? "unknown"}";
    }

    /// <summary>
    /// Runs a source and returns its header-safe value, or <c>null</c> if it is empty or throws.
    /// </summary>
    private static string? Attempt(Func<string?> source)
    {
        try
        {
            return ToHeaderSafe(source());
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Reduces a value to printable ASCII ("Étienne" becomes "Etienne"), or <c>null</c> if nothing remains.
    /// </summary>
    private static string? ToHeaderSafe(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;

        var builder = new StringBuilder();

        foreach (var character in value.Normalize(NormalizationForm.FormD))
        {
            if (character is >= ' ' and <= '~') builder.Append(character);
        }

        var result = builder.ToString().Trim();
        return result.Length > 0 ? result : null;
    }
}
