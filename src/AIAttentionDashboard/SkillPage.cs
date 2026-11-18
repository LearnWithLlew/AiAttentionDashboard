namespace AIAttentionDashboard;

/// <summary>The skill page that teaches an AI agent to use the dashboard well, embedded from SkillPage.md.</summary>
internal static class SkillPage
{
    public static string Read() => EmbeddedText.Read("SkillPage.md");
}

/// <summary>Instructions that let an AI agent install the /aiattentiondashboard command, its own way.</summary>
internal static class InstallPage
{
    public static string Read() => $"""
        # Install the /aiattentiondashboard command

        1. Ask the user: install it for this directory only, or globally?
        1. Install a command or skill named `aiattentiondashboard`, the way you (the AI agent) do it.
           Describe it as: "Start the AI Attention Dashboard, or do nothing if it is already running."
           Use everything below the line as its instructions.

        ---

        {EmbeddedText.Read("startup.process.md")}
        """;
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
