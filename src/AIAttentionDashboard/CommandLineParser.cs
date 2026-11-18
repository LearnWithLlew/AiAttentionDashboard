using System.CommandLine;
using System.Reflection;

namespace AIAttentionDashboard;

internal sealed class CommandLineParser(DashboardFiles files, IDashboardEnvironment environment)
{
    internal const string ServeOptionName = "--serve";

    // The package version, without the "+commit" suffix the SDK appends.
    internal static string Version { get; } =
        typeof(CommandLineParser).Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
            .InformationalVersion.Split('+')[0];

    public async Task<int> RunAsync(string[] args, TextWriter output, TextWriter error)
    {
        var commands = new (Option<bool> Option, Func<Task<int>> Run)[]
        {
            (new(ServeOptionName) { Description = "Run the dashboard server in this process.", Hidden = true }, ServeAsync),
            (FlagOption("--start", "Start the dashboard if needed and open it in a browser."), () => StartAsync(output)),
            (FlagOption("--refresh", "Reload the dashboard page in the browser."), () => RefreshAsync(output, error)),
            (FlagOption("--stop", "Stop the dashboard server."), () => StopAsync(output)),
            (FlagOption("--size", "Print the dashboard page's size, so you can make content that fits."), () => SizeAsync(output, error)),
            (FlagOption("--status", "Say whether the dashboard is running here, and what to do next."), () => StatusAsync(output)),
            (FlagOption("--skill", "Print a skill page for AI agents on how to use the dashboard well."), () => PrintAsync(output, SkillPage.Read())),
            (FlagOption("--install", "Print instructions for an AI agent to install the /aiattentiondashboard command."), () => PrintAsync(output, InstallPage.Read())),
        };

        var root = new RootCommand("Shows high-level action items for the user in a small browser window.");
        foreach (var (option, _) in commands)
        {
            root.Options.Add(option);
        }
        root.SetAction(async (parseResult, cancellationToken) =>
        {
            var chosen = commands.FirstOrDefault(command => parseResult.GetValue(command.Option));
            if (chosen.Run is not null)
            {
                return await chosen.Run();
            }
            error.WriteLine("Nothing to do. Pass --start, --refresh, --stop, --size, --status, --skill, or --install, or --help for usage.");
            return 1;
        });

        return await root.Parse(args).InvokeAsync(new InvocationConfiguration { Output = output, Error = error });
    }

    private static Option<bool> FlagOption(string name, string description) => new(name) { Description = description };

    private static Task<int> PrintAsync(TextWriter output, string page)
    {
        output.Write(page);
        return Task.FromResult(0);
    }

    private async Task<int> StartAsync(TextWriter output)
    {
        files.EnsureHtmlExists();
        var server = files.ReadServerInfo();
        var alreadyRunning = server is not null && await environment.IsServerRespondingAsync(server);
        if (!alreadyRunning)
        {
            server = await environment.LaunchServerAsync();
        }
        var pageAlreadyOpen = alreadyRunning && await environment.CountConnectedPagesAsync(server!) > 0;
        if (!pageAlreadyOpen)
        {
            environment.OpenBrowser(server!.BaseUri);
        }

        output.WriteLine($"""
            The AI Attention Dashboard is {(alreadyRunning ? "already running" : "started")} at {server!.BaseUri}{(pageAlreadyOpen ? " and already open in a browser" : "")}
            It shows the HTML file at: {files.HtmlPath}
            Keep it current by delegating it to a background subagent on a small, fast model.
            Tell the subagent to run `aiattentiondashboard --skill` and follow it, then message it each time you start, ask, or finish something.
            If you are that subagent, or can't start one, run `aiattentiondashboard --skill` and follow it yourself.
            """);
        return 0;
    }

    private async Task<int> RefreshAsync(TextWriter output, TextWriter error)
    {
        var server = files.ReadServerInfo();
        var reloaded = server is null ? null : await environment.RequestRefreshAsync(server);
        if (reloaded is null)
        {
            error.WriteLine("The dashboard is not running. Run `aiattentiondashboard --start` first.");
            return 1;
        }
        output.WriteLine(reloaded == 0
            ? "Refreshed, but no browser page is connected. Run `aiattentiondashboard --start` to open one."
            : $"Refreshed {reloaded} browser page(s).");
        return 0;
    }

    private async Task<int> SizeAsync(TextWriter output, TextWriter error)
    {
        var server = files.ReadServerInfo();
        var size = server is null ? null : await environment.RequestPageSizeAsync(server);
        if (size is null)
        {
            error.WriteLine("No dashboard page is open to measure. Run `aiattentiondashboard --start` first.");
            return 1;
        }
        output.WriteLine($"The dashboard page is {size.Width} wide and {size.Height} tall, in CSS pixels. Make content that fits without scrolling.");
        return 0;
    }

    private async Task<int> StatusAsync(TextWriter output)
    {
        var server = files.ReadServerInfo();
        var running = server is not null && await environment.IsServerRespondingAsync(server);
        output.WriteLine($"AI Attention Dashboard version {Version}");
        var latest = await environment.GetLatestVersionAsync();
        if (IsNewer(latest, Version))
        {
            output.WriteLine(environment.IsStandalone
                ? $"Version {latest} is available. Download it from https://github.com/LearnWithLlew/AiAttentionDashboard/releases/latest and replace the `aiattentiondashboard` on your PATH."
                : $"Version {latest} is available. Update with `dotnet tool update --global AIAttentionDashboard`.");
        }
        output.WriteLine(running
            ? $"""
              The AI Attention Dashboard is running at {server!.BaseUri}
              To keep it current, start a background subagent on a small, fast model.
              Tell it to run `aiattentiondashboard --skill` and follow it.
              """
            : """
              The AI Attention Dashboard is not running for this directory.
              To use it, run `aiattentiondashboard --start`.
              """);
        return 0;
    }

    private static bool IsNewer(string? candidate, string current) =>
        System.Version.TryParse(candidate, out var candidateVersion)
        && System.Version.TryParse(current.Split('-')[0], out var currentVersion)
        && candidateVersion > currentVersion;

    private async Task<int> StopAsync(TextWriter output)
    {
        var server = files.ReadServerInfo();
        var stopped = server is not null && await environment.RequestStopAsync(server);
        files.DeleteServerInfo();
        output.WriteLine(stopped ? "Stopped the dashboard." : "The dashboard was not running.");
        return 0;
    }

    private async Task<int> ServeAsync()
    {
        files.EnsureHtmlExists();
        await using var server = await DashboardServer.StartAsync(files);
        // The CLI owns server.json from here on. Deleting it on shutdown could delete a newer server's file.
        files.WriteServerInfo(new ServerInfo(server.Port, Environment.ProcessId));
        await server.WaitForShutdownAsync();
        return 0;
    }
}
