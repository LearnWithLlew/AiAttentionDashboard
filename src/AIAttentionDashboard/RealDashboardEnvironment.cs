using System.Diagnostics;
using System.Reflection;

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

    public async Task<ServerInfo> LaunchServerAsync()
    {
        files.DeleteServerInfo();
        var processPath = Environment.ProcessPath!;
        var isDotnetHost = Path.GetFileNameWithoutExtension(processPath).Equals("dotnet", StringComparison.OrdinalIgnoreCase);
        var startInfo = new ProcessStartInfo(processPath)
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WorkingDirectory = files.WorkingFolder,
        };
        if (isDotnetHost)
        {
            startInfo.ArgumentList.Add(Assembly.GetExecutingAssembly().Location);
        }
        startInfo.ArgumentList.Add(CommandLineParser.ServeOptionName);
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
}
