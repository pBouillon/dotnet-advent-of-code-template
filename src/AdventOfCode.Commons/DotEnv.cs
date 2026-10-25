namespace AdventOfCode.Commons;

/// <summary>
/// Reads values from <c>.env</c> files, looking in the application's folder and then in each of its parents.
/// </summary>
internal static class DotEnv
{
    /// <summary>
    /// The name of the file holding the values.
    /// </summary>
    public const string FileName = ".env";

    /// <summary>
    /// Returns the value of <paramref name="key"/> from the closest <c>.env</c> file that defines it, or <c>null</c> if none does.
    /// Unreadable files are ignored.
    /// </summary>
    public static string? Get(string key)
    {
        for (var directory = new DirectoryInfo(AppContext.BaseDirectory); directory is not null; directory = directory.Parent)
        {
            var path = Path.Combine(directory.FullName, FileName);

            if (File.Exists(path) && TryRead(path, key) is { } value) return value;
        }

        return null;
    }

    /// <summary>
    /// Returns the value of <paramref name="key"/> in the file at <paramref name="path"/>, or <c>null</c> if it is missing, empty or unreadable.
    /// </summary>
    private static string? TryRead(string path, string key)
    {
        try
        {
            foreach (var line in File.ReadLines(path))
            {
                var entry = line.Trim();

                if (entry.Length == 0 || entry.StartsWith('#')) continue;

                if (entry.StartsWith("export ", StringComparison.Ordinal))
                {
                    entry = entry["export ".Length..].TrimStart();
                }

                var separator = entry.IndexOf('=');
                if (separator <= 0 || entry[..separator].Trim() != key) continue;

                var value = Unquote(entry[(separator + 1)..].Trim());
                return value.Length > 0 ? value : null;
            }
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            return null;
        }

        return null;
    }

    /// <summary>
    /// Removes one pair of matching single or double quotes surrounding <paramref name="value"/>, if any.
    /// </summary>
    private static string Unquote(string value)
        => value.Length >= 2 && (value[0] is '"' or '\'') && value[^1] == value[0]
            ? value[1..^1]
            : value;
}
