# Architecture

## Processes

1. **CLI** (`aiattentiondashboard --start | --refresh | --stop`): short-lived, talks to the server over HTTP.
1. **Server** (`aiattentiondashboard --serve`, hidden): long-lived background process launched by `--start`.

## Files

All under `.local/aiattentiondashboard/`, relative to the working folder.

- `dashboard.html`: content Claude writes; the tool creates a stub only if it's missing.
- `server.json`: the server's port; present and answering `/ping` means "running".

## Server endpoints

Kestrel on `127.0.0.1`, random port.

- `GET /`: `dashboard.html` with a live-reload script injected before `</body>`.
- `GET /events`: Server-Sent Events stream the page listens on.
- `POST /refresh`: sends `refresh` to every open page and returns how many there were.
- `POST /stop`: shuts the server down.
- `GET /ping`: health check.

## Code

- `CommandLineParser`: System.CommandLine options, `--start`/`--refresh`/`--stop` logic.
- `IDashboardEnvironment`: the side-effect boundary (launch server, ping, refresh, stop, open browser).
- `RealDashboardEnvironment`: the production implementation.
- `DashboardServer`: Kestrel app and SSE broadcast.
- `DashboardFiles`: paths, stub HTML, `server.json`.
- `HtmlInjector`: adds the reload script.

## Tests

xunit.v3 + Verify.

- CLI behavior via a fake `IDashboardEnvironment`; snapshots capture output, exit code, and side-effect calls.
- `DashboardServer` tested in-process on a real port.
