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
        return Task.FromResult(new ServerInfo(5000, ProcessId: 5555));
    }

    public void OpenBrowser(Uri uri) => Calls.Add($"OpenBrowser({uri})");

    public Task<int?> RequestRefreshAsync(ServerInfo server)
    {
        Calls.Add($"RequestRefresh(port {server.Port})");
        return Task.FromResult(PagesToReload);
    }

    public int ConnectedPages { get; set; }

    public Task<int> CountConnectedPagesAsync(ServerInfo server)
    {
        Calls.Add($"CountConnectedPages(port {server.Port})");
        return Task.FromResult(ConnectedPages);
    }

    public PageSize? PageSize { get; set; }

    public Task<PageSize?> RequestPageSizeAsync(ServerInfo server)
    {
        Calls.Add($"RequestPageSize(port {server.Port})");
        return Task.FromResult(PageSize);
    }

    public bool ServerStops { get; set; }

    public Task<bool> RequestStopAsync(ServerInfo server)
    {
        Calls.Add($"RequestStop(port {server.Port})");
        return Task.FromResult(ServerStops);
    }
}
