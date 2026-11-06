namespace AIAttentionDashboard.Tests;

public class CommandLineTests : IDisposable
{
    private readonly string workingFolder = Path.Combine(Path.GetTempPath(), "aiattentiondashboard-tests-" + Guid.NewGuid());
    private readonly DashboardFiles files;
    private readonly FakeDashboardEnvironment environment = new();

    public CommandLineTests()
    {
        Directory.CreateDirectory(workingFolder);
        files = new DashboardFiles(workingFolder);
    }

    public void Dispose() => Directory.Delete(workingFolder, recursive: true);

    [Fact]
    public async Task Start_LaunchesServerAndOpensBrowser()
    {
        var result = await Run("--start");

        await Verify(result);
        Assert.True(File.Exists(files.HtmlPath));
    }

    [Fact]
    public async Task Start_WhenAlreadyRunning_DoesNotLaunchAgain()
    {
        files.EnsureHtmlExists();
        files.WriteServerInfo(6000);
        environment.ServerResponding = true;

        await Verify(await Run("--start"));
    }

    [Fact]
    public async Task Start_WhenServerInfoIsStale_LaunchesServer()
    {
        files.EnsureHtmlExists();
        files.WriteServerInfo(6000);
        environment.ServerResponding = false;

        await Verify(await Run("--start"));
    }

    [Fact]
    public async Task Start_DoesNotOverwriteExistingHtml()
    {
        files.EnsureHtmlExists();
        File.WriteAllText(files.HtmlPath, "<p>mine</p>");

        await Run("--start");

        Assert.Equal("<p>mine</p>", File.ReadAllText(files.HtmlPath));
    }

    [Fact]
    public async Task Refresh_WhenNeverStarted_Fails()
    {
        await Verify(await Run("--refresh"));
    }

    [Fact]
    public async Task Refresh_WhenServerUnreachable_Fails()
    {
        files.WriteServerInfo(6000);
        environment.PagesToReload = null;

        await Verify(await Run("--refresh"));
    }

    [Fact]
    public async Task Refresh_ReloadsConnectedPages()
    {
        files.WriteServerInfo(6000);
        environment.PagesToReload = 2;

        await Verify(await Run("--refresh"));
    }

    [Fact]
    public async Task Refresh_WithNoConnectedPages_SaysSo()
    {
        files.WriteServerInfo(6000);
        environment.PagesToReload = 0;

        await Verify(await Run("--refresh"));
    }

    [Fact]
    public async Task Stop_StopsRunningServer()
    {
        files.WriteServerInfo(6000);
        environment.ServerStops = true;

        await Verify(await Run("--stop"));
        Assert.False(File.Exists(files.ServerInfoPath));
    }

    [Fact]
    public async Task Stop_WhenNeverStarted_SaysSo()
    {
        await Verify(await Run("--stop"));
    }

    [Fact]
    public async Task Stop_WhenServerUnreachable_CleansUpStaleServerInfo()
    {
        files.WriteServerInfo(6000);
        environment.ServerStops = false;

        await Verify(await Run("--stop"));
        Assert.False(File.Exists(files.ServerInfoPath));
    }

    [Fact]
    public async Task NoOptions_ExplainsUsage()
    {
        await Verify(await Run());
    }

    private async Task<object> Run(params string[] args)
    {
        var output = new StringWriter();
        var error = new StringWriter();
        var exitCode = await new CommandLineParser(files, environment).RunAsync(args, output, error);
        return new
        {
            ExitCode = exitCode,
            Output = Scrub(output.ToString()),
            Error = Scrub(error.ToString()),
            environment.Calls,
        };
    }

    private string Scrub(string text) => text.Replace(workingFolder, "{WorkingFolder}").Replace('\\', '/');
}
