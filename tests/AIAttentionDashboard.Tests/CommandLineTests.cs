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
        files.WriteServerInfo(new ServerInfo(6000, ProcessId: 1234));
        environment.ServerResponding = true;

        await Verify(await Run("--start"));
    }

    [Fact]
    public async Task Start_WhenPageAlreadyOpen_DoesNotOpenAnother()
    {
        files.EnsureHtmlExists();
        files.WriteServerInfo(new ServerInfo(6000, ProcessId: 1234));
        environment.ServerResponding = true;
        environment.ConnectedPages = 1;

        await Verify(await Run("--start"));
    }

    [Fact]
    public async Task Start_WhenServerInfoIsStale_LaunchesServer()
    {
        files.EnsureHtmlExists();
        files.WriteServerInfo(new ServerInfo(6000, ProcessId: 1234));
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
        files.WriteServerInfo(new ServerInfo(6000, ProcessId: 1234));
        environment.PagesToReload = null;

        await Verify(await Run("--refresh"));
    }

    [Fact]
    public async Task Refresh_ReloadsConnectedPages()
    {
        files.WriteServerInfo(new ServerInfo(6000, ProcessId: 1234));
        environment.PagesToReload = 2;

        await Verify(await Run("--refresh"));
    }

    [Fact]
    public async Task Refresh_WithNoConnectedPages_SaysSo()
    {
        files.WriteServerInfo(new ServerInfo(6000, ProcessId: 1234));
        environment.PagesToReload = 0;

        await Verify(await Run("--refresh"));
    }

    [Fact]
    public async Task Stop_StopsRunningServer()
    {
        files.WriteServerInfo(new ServerInfo(6000, ProcessId: 1234));
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
        files.WriteServerInfo(new ServerInfo(6000, ProcessId: 1234));
        environment.ServerStops = false;

        await Verify(await Run("--stop"));
        Assert.False(File.Exists(files.ServerInfoPath));
    }

    [Fact]
    public async Task Size_ReportsPageSize()
    {
        files.WriteServerInfo(new ServerInfo(6000, ProcessId: 1234));
        environment.PageSize = new PageSize(1200, 300);

        await Verify(await Run("--size"));
    }

    [Fact]
    public async Task Size_WhenNoPageOpen_Fails()
    {
        files.WriteServerInfo(new ServerInfo(6000, ProcessId: 1234));
        environment.PageSize = null;

        await Verify(await Run("--size"));
    }

    [Fact]
    public async Task Size_WhenNeverStarted_Fails()
    {
        await Verify(await Run("--size"));
    }

    [Fact]
    public async Task Skill_PrintsSkillPage()
    {
        var output = new StringWriter();
        var error = new StringWriter();

        var exitCode = await new CommandLineParser(files, environment).RunAsync(["--skill"], output, error);

        Assert.Equal(0, exitCode);
        Assert.Equal("", error.ToString());
        Assert.Empty(environment.Calls);
        await Verify(output.ToString(), "md");
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
