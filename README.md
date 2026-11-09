# ![AI Attention Dashboard icon](https://raw.githubusercontent.com/LearnWithLlew/AiAttentionDashboard/master/icon.png) AI Attention Dashboard

[![build-and-test](https://github.com/LearnWithLlew/AiAttentionDashboard/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/LearnWithLlew/AiAttentionDashboard/actions/workflows/build-and-test.yml)
[![NuGet](https://img.shields.io/nuget/v/AIAttentionDashboard.svg)](https://www.nuget.org/packages/AIAttentionDashboard)
[![NuGet downloads](https://img.shields.io/nuget/dt/AIAttentionDashboard.svg)](https://www.nuget.org/packages/AIAttentionDashboard)

A small browser window where your AI coding agent tells you what needs your attention.
Instead of scrolling the terminal to find out whether Claude is working, waiting on you, or done, you glance at the dashboard.

The agent writes an HTML file and the dashboard reloads to show it.
The tool only serves and refreshes the page; the agent decides what goes on it.

![The dashboard (middle right) showing Claude's checklist next to the Claude Code terminal](https://raw.githubusercontent.com/LearnWithLlew/AiAttentionDashboard/master/docs/images/AiAttentionDashboard.screenshot.png)

## Install

### Standalone download

If you do not want to install the application via .NET, you'll find pre-built binaries on the [latest release](https://github.com/LearnWithLlew/AiAttentionDashboard/releases/latest) page.

Get the archive for your platform (`osx-arm64`, `osx-x64`, `linux-x64`, `linux-arm64`, `win-x64`, or `win-arm64`), extract it, and put `aiattentiondashboard` on your `PATH`.

The binaries are unsigned, so Windows SmartScreen and macOS Gatekeeper may warn you the first time you run one.

#### Standalone download via gh(1)

The [GitHub CLI](https://cli.github.com/) (`gh`) command makes this easier.

Example: fetch only the Apple silicon version for macOS, into `.local`, a directory git already ignores.

Run this from the project root.

```bash
mkdir -p .local/bin
gh release download --repo LearnWithLlew/AiAttentionDashboard --pattern 'aiattentiondashboard-osx-arm64.tar.gz' --dir .local
tar -xzf .local/aiattentiondashboard-osx-arm64.tar.gz -C .local/bin
ln -sf "$PWD/.local/bin/aiattentiondashboard" ~/bin/aiattentiondashboard
```

`ln -sf` replaces an existing link.
Put `~/bin` on your `PATH` if it isn't already.

#### macOS

If macOS refuses to open a binary you downloaded in a browser, clear the quarantine flag that browsers add. In Terminal, replace the path with where you put the binary:

```bash
xattr -dr com.apple.quarantine /path/to/aiattentiondashboard
```

#### Windows (untested)

These steps are not yet confirmed on Windows.

If SmartScreen says "Windows protected your PC", click **More info**, then **Run anyway**.

Unblocking the file in PowerShell may also work: `Unblock-File /path/to/aiattentiondashboard.exe`.

### .NET tool

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
