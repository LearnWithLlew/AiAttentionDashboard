namespace AIAttentionDashboard;

/// <summary>The skill page that teaches an AI agent to use the dashboard well, embedded from SkillPage.md.</summary>
internal static class SkillPage
{
    public static string Read()
    {
        using var stream = typeof(SkillPage).Assembly.GetManifestResourceStream("SkillPage.md")
            ?? throw new InvalidOperationException("The skill page resource is missing.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
