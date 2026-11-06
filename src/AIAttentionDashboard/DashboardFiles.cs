using System.Text.Json;

namespace AIAttentionDashboard;

/// <summary>Where the dashboard keeps its files, relative to the folder the CLI runs in.</summary>
internal sealed class DashboardFiles(string workingFolder)
{
    public string WorkingFolder { get; } = workingFolder;

    public string Folder => Path.Combine(WorkingFolder, ".local", "aiattentiondashboard");

    public string HtmlPath => Path.Combine(Folder, "dashboard.html");

    public string ServerInfoPath => Path.Combine(Folder, "server.json");

    public void EnsureHtmlExists()
    {
        EnsureFolderExists();
        if (!File.Exists(HtmlPath))
        {
            File.WriteAllText(HtmlPath, StubHtml);
        }
    }

    public void WriteServerInfo(ServerInfo server)
    {
        EnsureFolderExists();
        File.WriteAllText(ServerInfoPath, JsonSerializer.Serialize(server));
    }

    /// <summary>The folder ignores itself, so the dashboard's files never show up in the user's repo.</summary>
    private void EnsureFolderExists()
    {
        Directory.CreateDirectory(Folder);
        var gitignore = Path.Combine(Folder, ".gitignore");
        if (!File.Exists(gitignore))
        {
            File.WriteAllText(gitignore, "*\n");
        }
    }

    public ServerInfo? ReadServerInfo() =>
        File.Exists(ServerInfoPath)
            ? JsonSerializer.Deserialize<ServerInfo>(File.ReadAllText(ServerInfoPath))
            : null;

    public void DeleteServerInfo()
    {
        if (File.Exists(ServerInfoPath))
        {
            File.Delete(ServerInfoPath);
        }
    }

    internal const string StubHtml = """
        <!DOCTYPE html>
        <html>
        <head><meta charset="utf-8"><title>AI Attention Dashboard</title></head>
        <body>
        <p>Nothing needs your attention yet.</p>
        </body>
        </html>
        """;
}

internal sealed record ServerInfo(int Port, int ProcessId)
{
    public Uri BaseUri => new($"http://127.0.0.1:{Port}/");
}
