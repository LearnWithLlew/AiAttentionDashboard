# --status

`aiattentiondashboard --status` takes no arguments.

## What it shows

- The version of the AI Attention Dashboard.
- How to update, if NuGet has a newer version.
  The .NET tool gets a `dotnet tool update` command.
  The standalone download gets a link to the latest release.
  If NuGet can't be reached, it says nothing about updating.
- Whether a dashboard server is running for the current directory.

## What it tells Claude

- If running: to keep the dashboard current, start a subagent and have it run `aiattentiondashboard --skill`.
  The subagent works out what to do from there.
- If not running: run `aiattentiondashboard --start`.

## Sample output

When running and up to date:

```
AI Attention Dashboard version 0.1.7
The AI Attention Dashboard is running at http://127.0.0.1:5123/
To keep it current, start a background subagent on a small, fast model.
Tell it to run `aiattentiondashboard --skill` and follow it.
```

When not running, with an update out (.NET tool):

```
AI Attention Dashboard version 0.1.0
Version 0.1.7 is available. Update with `dotnet tool update --global AIAttentionDashboard`.
The AI Attention Dashboard is not running for this directory.
To use it, run `aiattentiondashboard --start`.
```

With an update out (standalone):

```
Version 0.1.7 is available. Download it from https://github.com/LearnWithLlew/AiAttentionDashboard/releases/latest and replace the `aiattentiondashboard` on your PATH.
```
