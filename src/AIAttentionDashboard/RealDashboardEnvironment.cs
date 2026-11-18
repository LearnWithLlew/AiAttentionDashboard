using System.Diagnostics;
using System.Net.Http.Json;

namespace AIAttentionDashboard;

internal sealed class RealDashboardEnvironment(DashboardFiles files) : IDashboardEnvironment
{
    private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(2) };

    public async Task<bool> IsServerRespondingAsync(ServerInfo server)
    {
        try
        {
            return (await Http.GetAsync(new Uri(server.BaseUri, "ping"))).IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            return false;
        }
        catch (TaskCanceledException)
        {
            return false;
        }
    }

    public bool IsStandalone => InstallKind.IsStandalone;

    public async Task<string?> GetLatestVersionAsync()
    {
        try
        {
            var index = await Http.GetFromJsonAsync<NuGetVersionIndex>("https://api.nuget.org/v3-flatcontainer/aiattentiondashboard/index.json");
            // Versions are listed oldest first; skip prereleases such as "1.0.0-beta".
            return index?.Versions.LastOrDefault(v => !v.Contains('-'));
        }
        catch (HttpRequestException)
        {
            return null;
        }
        catch (TaskCanceledException)
        {
            return null;
        }
    }

    private sealed record NuGetVersionIndex(string[] Versions);

    public async Task<ServerInfo> LaunchServerAsync()
    {
        files.DeleteServerInfo();
        var startInfo = BuildServerStartInfo();
        using var process = Process.Start(startInfo) ?? throw new InvalidOperationException("Could not start the dashboard server.");

        for (var attempt = 0; attempt < 100; attempt++)
        {
            if (process.HasExited)
            {
                throw new InvalidOperationException("The dashboard server exited during startup.");
            }
            var info = files.ReadServerInfo();
            if (info is not null && await IsServerRespondingAsync(info))
            {
                return info;
            }
            await Task.Delay(100);
        }
        throw new TimeoutException("The dashboard server did not start within 10 seconds.");
    }

    private ProcessStartInfo BuildServerStartInfo()
    {
        var startInfo = new ProcessStartInfo(Environment.ProcessPath!)
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WorkingDirectory = files.WorkingFolder,
        };
        InstallKind.AddHostArguments(startInfo);
        startInfo.ArgumentList.Add(CommandLineParser.ServeOptionName);
        return startInfo;
    }

    public void OpenBrowser(Uri uri) =>
        Process.Start(new ProcessStartInfo(uri.ToString()) { UseShellExecute = true })?.Dispose();

    public async Task<int?> RequestRefreshAsync(ServerInfo server)
    {
        try
        {
            var response = await Http.PostAsync(new Uri(server.BaseUri, "refresh"), content: null);
            response.EnsureSuccessStatusCode();
            return int.Parse(await response.Content.ReadAsStringAsync());
        }
        catch (HttpRequestException)
        {
            return null;
        }
        catch (TaskCanceledException)
        {
            return null;
        }
    }

    public async Task<int> CountConnectedPagesAsync(ServerInfo server)
    {
        try
        {
            return int.Parse(await Http.GetStringAsync(new Uri(server.BaseUri, "pages")));
        }
        catch (HttpRequestException)
        {
            return 0;
        }
        catch (TaskCanceledException)
        {
            return 0;
        }
    }

    public async Task<PageSize?> RequestPageSizeAsync(ServerInfo server)
    {
        try
        {
            return await Http.GetFromJsonAsync<PageSize>(new Uri(server.BaseUri, "size"));
        }
        catch (HttpRequestException)
        {
            return null;
        }
        catch (TaskCanceledException)
        {
            return null;
        }
    }

    public async Task<bool> RequestStopAsync(ServerInfo server)
    {
        try
        {
            if (!(await Http.PostAsync(new Uri(server.BaseUri, "stop"), content: null)).IsSuccessStatusCode)
            {
                return false;
            }
        }
        catch (HttpRequestException)
        {
            return false;
        }
        catch (TaskCanceledException)
        {
            return false;
        }
        await WaitForExitAsync(server.ProcessId);
        return true;
    }

    private static async Task WaitForExitAsync(int processId)
    {
        Process process;
        try
        {
            process = Process.GetProcessById(processId);
        }
        catch (ArgumentException)
        {
            return; // Already exited.
        }
        using (process)
        {
            // The server runs the same executable as this CLI; anything else means the ID was reused.
            if (process.ProcessName != Process.GetCurrentProcess().ProcessName)
            {
                return;
            }
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            try
            {
                await process.WaitForExitAsync(timeout.Token);
            }
            catch (OperationCanceledException)
            {
                // Best effort: stop waiting after 10 seconds and let the caller carry on.
            }
        }
    }
}
