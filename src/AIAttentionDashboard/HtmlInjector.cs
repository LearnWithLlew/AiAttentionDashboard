namespace AIAttentionDashboard;

/// <summary>Adds the live-reload script to the dashboard page when it is served.</summary>
internal static class HtmlInjector
{
    internal const string ReloadScript = """
        <script>
        {
            new EventSource('/events').addEventListener('refresh', () => location.reload());
            const reportSize = () => fetch('/size', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ width: innerWidth, height: innerHeight }),
            });
            let resizeTimer;
            addEventListener('resize', () => { clearTimeout(resizeTimer); resizeTimer = setTimeout(reportSize, 200); });
            reportSize();
        }
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
