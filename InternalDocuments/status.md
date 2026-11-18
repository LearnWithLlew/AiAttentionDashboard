# --status

`aiattentiondashboard --status` takes no arguments.

## What it shows

- Whether a dashboard server is running for the current directory.

## What it tells Claude

- If running: to keep the dashboard current, start a subagent and have it run `aiattentiondashboard --skill`.
  The subagent works out what to do from there.
- If not running: run `aiattentiondashboard --start`.

## Sample output

When running:

```
The AI Attention Dashboard is running at http://localhost:5123/
To keep it current, start a background subagent on a small, fast model.
Tell it to run `aiattentiondashboard --skill` and follow it.
```

When not running:

```
The AI Attention Dashboard is not running for this directory.
To use it, run `aiattentiondashboard --start`.
```
