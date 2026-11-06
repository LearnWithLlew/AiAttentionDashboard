using System.Collections.Concurrent;
using System.Threading.Channels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AIAttentionDashboard;

/// <summary>Serves the dashboard HTML and tells open pages to reload on request.</summary>
internal sealed class DashboardServer : IAsyncDisposable
{
    private readonly WebApplication app;
    private readonly ConcurrentDictionary<Channel<string>, byte> listeners = new();

    private DashboardServer(WebApplication app, int port)
    {
        this.app = app;
        Port = port;
    }

    public int Port { get; }

    public static async Task<DashboardServer> StartAsync(DashboardFiles files)
    {
        var builder = WebApplication.CreateSlimBuilder();
        builder.Logging.ClearProviders();
        builder.WebHost.UseUrls("http://127.0.0.1:0");
        var app = builder.Build();

        DashboardServer? server = null;
        app.MapGet("/", async context =>
        {
            files.EnsureHtmlExists();
            context.Response.ContentType = "text/html; charset=utf-8";
            await context.Response.WriteAsync(HtmlInjector.Inject(await File.ReadAllTextAsync(files.HtmlPath)));
        });
        app.MapGet("/ping", () => "pong");
        app.MapGet("/events", context => server!.StreamEventsAsync(context));
        app.MapPost("/refresh", () => Results.Ok(server!.Broadcast()));
        app.MapPost("/stop", () =>
        {
            app.Lifetime.StopApplication();
            return Results.Ok();
        });

        await app.StartAsync();
        var address = app.Services.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>()!.Addresses.First();
        server = new DashboardServer(app, new Uri(address).Port);
        return server;
    }

    public Task WaitForShutdownAsync() => app.WaitForShutdownAsync();

    /// <returns>How many open pages were told to reload.</returns>
    private int Broadcast()
    {
        var count = 0;
        foreach (var listener in listeners.Keys)
        {
            if (listener.Writer.TryWrite("refresh"))
            {
                count++;
            }
        }
        return count;
    }

    private async Task StreamEventsAsync(HttpContext context)
    {
        context.Response.ContentType = "text/event-stream";
        context.Response.Headers.CacheControl = "no-cache";
        var channel = Channel.CreateUnbounded<string>();
        listeners[channel] = 0;
        try
        {
            await context.Response.WriteAsync(": connected\n\n");
            await context.Response.Body.FlushAsync();
            await foreach (var name in channel.Reader.ReadAllAsync(context.RequestAborted))
            {
                await context.Response.WriteAsync($"event: {name}\ndata: {name}\n\n");
                await context.Response.Body.FlushAsync();
            }
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            listeners.TryRemove(channel, out _);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await app.StopAsync();
        await app.DisposeAsync();
    }
}
