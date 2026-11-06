using System.CommandLine;

namespace AIAttentionDashboard;

internal sealed class CommandLineParser(DashboardFiles files, IDashboardEnvironment environment)
{
    internal const string ServeOptionName = "--serve";

    public async Task<int> RunAsync(string[] args, TextWriter output, TextWriter error)
    {
        var start = new Option<bool>("--start") { Description = "Start the dashboard if needed and open it in a browser." };
        var refresh = new Option<bool>("--refresh") { Description = "Reload the dashboard page in the browser." };
        var stop = new Option<bool>("--stop") { Description = "Stop the dashboard server." };
        var skill = new Option<bool>("--skill") { Description = "Print a skill page for AI agents on how to use the dashboard well." };
        var serve = new Option<bool>(ServeOptionName) { Description = "Run the dashboard server in this process.", Hidden = true };

        var root = new RootCommand("Shows high-level action items for the user in a small browser window.");
        root.Options.Add(start);
        root.Options.Add(refresh);
        root.Options.Add(stop);
        root.Options.Add(skill);
        root.Options.Add(serve);
        root.SetAction(async (parseResult, cancellationToken) =>
        {
            if (parseResult.GetValue(serve))
            {
                return await ServeAsync();
            }
            if (parseResult.GetValue(start))
            {
                return await StartAsync(output);
            }
            if (parseResult.GetValue(refresh))
            {
                return await RefreshAsync(output, error);
            }
            if (parseResult.GetValue(stop))
            {
                return await StopAsync(output);
            }
            if (parseResult.GetValue(skill))
            {
                output.Write(SkillPage.Read());
                return 0;
            }
            error.WriteLine("Nothing to do. Pass --start, --refresh, --stop, or --skill, or --help for usage.");
            return 1;
        });

        return await root.Parse(args).InvokeAsync(new InvocationConfiguration { Output = output, Error = error });
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
            Write high-level action items for the user into that file, then run `aiattentiondashboard --refresh` to reload the browser.
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
        files.WriteServerInfo(server.Port);
        try
        {
            await server.WaitForShutdownAsync();
        }
        finally
        {
            files.DeleteServerInfo();
        }
        return 0;
    }
}
