using System.Diagnostics;

namespace AdventOfCode.Commons;

/// <summary>
/// Reads values from the git configuration.
/// </summary>
internal static class GitConfig
{
    /// <summary>
    /// Maximum time to wait for git before giving up.
    /// </summary>
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(2);

    /// <summary>
    /// Returns a git config key's value, or <c>null</c> if it is unset, git fails or times out.
    /// Runs from the app directory so a repo-local identity wins; throws if git can't start.
    /// </summary>
    public static string? Get(string key)
    {
        var startInfo = new ProcessStartInfo("git", ["config", "--get", key])
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = AppContext.BaseDirectory,
        };

        using var process = Process.Start(startInfo);
        if (process is null) return null;

        var output = process.StandardOutput.ReadToEndAsync();
        _ = process.StandardError.ReadToEndAsync();

        if (!process.WaitForExit(Timeout))
        {
            process.Kill();
            return null;
        }

        var value = output.GetAwaiter().GetResult().Trim();
        return process.ExitCode == 0 && value.Length > 0 ? value : null;
    }
}
