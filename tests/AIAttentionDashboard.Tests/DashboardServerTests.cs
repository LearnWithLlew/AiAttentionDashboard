namespace AIAttentionDashboard.Tests;

public class DashboardServerTests : IAsyncLifetime
{
    private readonly string workingFolder = Path.Combine(Path.GetTempPath(), "aiattentiondashboard-tests-" + Guid.NewGuid());
    private readonly HttpClient http = new();
    private DashboardServer server = null!;
    private Uri baseUri = null!;

    public async ValueTask InitializeAsync()
    {
        var files = new DashboardFiles(workingFolder);
        files.EnsureHtmlExists();
        server = await DashboardServer.StartAsync(files);
        baseUri = new ServerInfo(server.Port).BaseUri;
    }

    public async ValueTask DisposeAsync()
    {
        http.Dispose();
        await server.DisposeAsync();
        Directory.Delete(workingFolder, recursive: true);
    }

    [Fact]
    public async Task ServesDashboardWithReloadScript()
    {
        await Verify(await http.GetStringAsync(baseUri), "html");
    }

    [Fact]
    public async Task Refresh_WithNoPages_ReloadsZero()
    {
        var response = await http.PostAsync(new Uri(baseUri, "refresh"), null);

        Assert.Equal("0", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Refresh_SendsEventToConnectedPage()
    {
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        using var response = await http.GetAsync(new Uri(baseUri, "events"), HttpCompletionOption.ResponseHeadersRead, timeout.Token);
        using var reader = new StreamReader(await response.Content.ReadAsStreamAsync(timeout.Token));
        await reader.ReadLineAsync(timeout.Token); // ": connected"
        await reader.ReadLineAsync(timeout.Token); // blank line ending the comment event

        var reloaded = await (await http.PostAsync(new Uri(baseUri, "refresh"), null, timeout.Token)).Content.ReadAsStringAsync(timeout.Token);

        Assert.Equal("1", reloaded);
        Assert.Equal("event: refresh", await reader.ReadLineAsync(timeout.Token));
    }
}
