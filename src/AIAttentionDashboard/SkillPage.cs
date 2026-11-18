namespace AIAttentionDashboard;

/// <summary>The skill page that teaches an AI agent to use the dashboard well, embedded from SkillPage.md.</summary>
internal static class SkillPage
{
    public static string Read() => EmbeddedText.Read("SkillPage.md");
}

/// <summary>Text files embedded in the tool when it is built.</summary>
internal static class EmbeddedText
{
    public static string Read(string logicalName)
    {
        using var stream = typeof(EmbeddedText).Assembly.GetManifestResourceStream(logicalName)
            ?? throw new InvalidOperationException($"The embedded resource {logicalName} is missing.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
