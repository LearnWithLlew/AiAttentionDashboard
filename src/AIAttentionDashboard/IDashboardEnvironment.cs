namespace AIAttentionDashboard;

/// <summary>The side effects the CLI needs: talking to the server process and the browser.</summary>
internal interface IDashboardEnvironment
{
    Task<bool> IsServerRespondingAsync(ServerInfo server);

    /// <summary>Starts the background server process and waits until it has published its port.</summary>
    Task<ServerInfo> LaunchServerAsync();

    void OpenBrowser(Uri uri);

    /// <returns>How many open pages were told to reload, or null if the server could not be reached.</returns>
    Task<int?> RequestRefreshAsync(ServerInfo server);
}
