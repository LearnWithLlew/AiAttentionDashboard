# AIAttentionDashboard

## What it is

This is a CLI that allows for three main things:

1. Opens and refreshes an HTML page in a browser
1. Gives a mechanical help page for Claude
1. Gives a skill help page for Claude on how to interact with it

## Desktop layout

The goal is a desktop where Claude talks to the user in a small browser window on the right.
Claude shows only high-level action items there.
Each item must fit easily in the small window without scrolling.

The layout has four windows:

1. A Claude Code terminal fills the left half of the screen.
1. A PowerShell terminal sits at the top right.
1. The small dashboard browser sits in the middle right.
1. A second Claude Code terminal sits at the bottom right.

The dashboard browser is short and wide, about one fifth of the screen height.
It is the only window the user needs to glance at to know what needs attention.

## HTML file location

The HTML page lives at `.local/aiattentiondashboard/dashboard.html`, relative to the folder where `--start` runs.
The `.local/` folder is listed in `.gitignore`, so the page is never committed.
`--start` should give a message to Claude saying where the HTML file is located.

## Basics

```
aiattentiondashboard --start
```

This starts in this folder if it's not already started, and launches the HTML page in a browser.

It also gives a small prompt back to Claude so that it understands how to get more information about where the HTML file it reads comes from.

```
--refresh
```

Refreshes the HTML page in the browser to show the latest content.

```
--size
```

Tells the size of the current browser window.

```
--help
```

```
--skill
```

Gives a skill page to Claude on how to effectively create things for the user.

```
--speak
```

Allows Claude to speak out loud the content of the HTML page.