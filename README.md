# ![](https://raw.githubusercontent.com/LearnWithLlew/AiAttentionDashboard/master/icon.png) AI Attention Dashboard

[![build-and-test](https://github.com/LearnWithLlew/AiAttentionDashboard/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/LearnWithLlew/AiAttentionDashboard/actions/workflows/build-and-test.yml)
[![Check Markdown links](https://github.com/LearnWithLlew/AiAttentionDashboard/actions/workflows/markdown-link-check.yml/badge.svg)](https://github.com/LearnWithLlew/AiAttentionDashboard/actions/workflows/markdown-link-check.yml)
[![publish](https://github.com/LearnWithLlew/AiAttentionDashboard/actions/workflows/publish.yml/badge.svg)](https://github.com/LearnWithLlew/AiAttentionDashboard/actions/workflows/publish.yml)
[![NuGet](https://img.shields.io/nuget/v/AIAttentionDashboard.svg)](https://www.nuget.org/packages/AIAttentionDashboard)
[![NuGet downloads](https://img.shields.io/nuget/dt/AIAttentionDashboard.svg)](https://www.nuget.org/packages/AIAttentionDashboard)

A small browser window where your AI coding agent tells you what needs your attention.
Instead of scrolling the terminal to find out whether Claude is working, waiting on you, or done, you glance at the dashboard.

The agent writes an HTML file and the dashboard reloads to show it.
The tool only serves and refreshes the page; the agent decides what goes on it.

![The dashboard (middle right) showing Claude's checklist next to the Claude Code terminal](https://raw.githubusercontent.com/LearnWithLlew/AiAttentionDashboard/master/docs/images/AiAttentionDashboard.screenshot.png)

## Install

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet tool install --global AIAttentionDashboard
```

This installs the `aiattentiondashboard` command.

## Use

Run these from your project's folder:

| Command | What it does |
| - | - |
| `aiattentiondashboard --start` | Starts the dashboard if needed, opens it in a browser, and prints the HTML file to write. |
| `aiattentiondashboard --refresh` | Reloads the page after the HTML file changes. |
| `aiattentiondashboard --size` | Prints the page's width and height, so content can fit without scrolling. |
| `aiattentiondashboard --stop` | Stops the dashboard. |
| `aiattentiondashboard --skill` | Prints instructions for the AI agent on how to use the dashboard well. |

The quickest way to get going is to tell your agent:

> Run `aiattentiondashboard --skill` and follow it.

## Files it creates

The dashboard keeps its files in `.local/aiattentiondashboard/` under the folder you run it from:

- `dashboard.html`: the page the agent writes.
- `server.json`: the running server's port and process ID.
- `.gitignore`: ignores everything in that folder, so git never shows these files.

## License

[MIT](LICENSE)
