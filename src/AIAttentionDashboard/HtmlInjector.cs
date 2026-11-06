namespace AIAttentionDashboard;

/// <summary>Adds the live-reload script to the dashboard page when it is served.</summary>
internal static class HtmlInjector
{
    internal const string ReloadScript = """
        <script>
        new EventSource('/events').addEventListener('refresh', () => location.reload());
        </script>
        """;

    public static string Inject(string html)
    {
        var index = html.LastIndexOf("</body>", StringComparison.OrdinalIgnoreCase);
        return index < 0
            ? html + Environment.NewLine + ReloadScript
            : html.Insert(index, ReloadScript + Environment.NewLine);
    }
}
