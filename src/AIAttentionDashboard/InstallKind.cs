using System.Diagnostics;
using System.Reflection;

namespace AIAttentionDashboard;

/// <summary>
/// What differs between the standalone download and the .NET tool.
/// The .csproj defines STANDALONE when publishing the single-file standalone download.
/// </summary>
internal static class InstallKind
{
#if STANDALONE
    public static bool IsStandalone => true;

    // A single-file app is never run by the dotnet host, so there is nothing to add.
    public static void AddHostArguments(ProcessStartInfo startInfo)
    {
    }
#else
    public static bool IsStandalone => false;

    public static void AddHostArguments(ProcessStartInfo startInfo)
    {
        if (IsDotnetHost(startInfo.FileName))
        {
            startInfo.ArgumentList.Add(Assembly.GetExecutingAssembly().Location);
        }
    }

    private static bool IsDotnetHost(string processPath) =>
        Path.GetFileNameWithoutExtension(processPath).Equals("dotnet", StringComparison.OrdinalIgnoreCase);
#endif
}
