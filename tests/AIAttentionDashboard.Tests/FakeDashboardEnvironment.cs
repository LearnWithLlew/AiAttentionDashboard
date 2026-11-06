namespace AIAttentionDashboard.Tests;

internal sealed class FakeDashboardEnvironment : IDashboardEnvironment
{
    public List<string> Calls { get; } = [];

    public bool ServerResponding { get; set; }

    public int? PagesToReload { get; set; } = 1;

    public Task<bool> IsServerRespondingAsync(ServerInfo server)
    {
        Calls.Add($"IsServerResponding(port {server.Port})");
        return Task.FromResult(ServerResponding);
    }

    public Task<ServerInfo> LaunchServerAsync()
    {
        Calls.Add("LaunchServer");
        return Task.FromResult(new ServerInfo(5000));
    }

    public void OpenBrowser(Uri uri) => Calls.Add($"OpenBrowser({uri})");

    public Task<int?> RequestRefreshAsync(ServerInfo server)
    {
        Calls.Add($"RequestRefresh(port {server.Port})");
        return Task.FromResult(PagesToReload);
    }
}
