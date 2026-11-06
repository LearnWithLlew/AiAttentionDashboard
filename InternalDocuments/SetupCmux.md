# Running the Dashboard in cmux

How to run the dashboard server in a cmux pane and see the dashboard as a browser tab in that same pane.

## 1. Enable the cmux browser

cmux's built-in browser is off by default.

```bash
cmux browser-status   # prints enabled or disabled
cmux enable-browser
```

Undo it with `cmux disable-browser`.

## 2. Find the pane

`cmux tree` lists panes but not where they are on screen.
To find a pane by position (for example "middle right"), read the pixel frames:

```bash
cmux --json list-panes --workspace workspace:8
```

Panes with the same `x` form a column.
Within a column, sort by `y`: the smallest is the top.
Note the pane's `ref` (for example `pane:20`) and its terminal surface (for example `surface:22`).

## 3. Start the server in that pane

Install the tool once, if you haven't:

```bash
dotnet tool install --global AIAttentionDashboard
```

Then send the command to the pane's terminal, from the folder whose dashboard you want (here, the repo root):

```bash
cmux send --workspace workspace:8 --surface surface:22 'cd ~/Github/AiAttentionDashboard && aiattentiondashboard --serve
'
```

The trailing newline inside the quotes presses Enter.

The dashboard's files (`server.json`, `dashboard.html`) are in `.local/aiattentiondashboard/` under that folder.
Run the other commands from the same folder, or they won't find the server.

`--serve` does not write `server.json` until the server is up, and does not print its port.
Get the port from `server.json`, or from:

```bash
lsof -nP -iTCP -sTCP:LISTEN | grep -i aiatten
```

## 4. Open the dashboard as a browser tab

Add a browser surface to the same pane:

```bash
cmux new-surface --type browser --pane pane:20 --workspace workspace:8 --url http://127.0.0.1:<port>/ --focus false
```

The pane now has two tabs: the server's terminal and the "AI Attention Dashboard" page.
Check with:

```bash
cmux list-pane-surfaces --workspace workspace:8 --pane pane:20
```

If the browser tab does not show up, the cmux browser is probably still disabled (see step 1).

## 5. Use it

From the folder you started the server in:

- `aiattentiondashboard --size` prints the page size, which also confirms the page is connected.
- `aiattentiondashboard --refresh` reloads the page after you edit `dashboard.html`.
- `aiattentiondashboard --stop` stops the server, and the dashboard tab closes itself.

Ctrl-C in the server's terminal tab also stops it.

## Why `--serve` and not `--start`?

`--serve` is hidden from `--help`, but it runs the server in the pane's terminal, where you can see it and stop it.

`--start` launches the server and opens the page in your system browser, not in cmux.
If a page is already connected, `--start` skips opening a browser.
So you can open the cmux tab first, as above, and `--start` will still report the running server.

## Testing local changes

To try unreleased code, run the server from source instead:

```bash
dotnet run --project src/AIAttentionDashboard -- --serve
```

`dotnet run` runs from `src/AIAttentionDashboard/`, so the files are in `src/AIAttentionDashboard/.local/aiattentiondashboard/`.
Run the other commands as `dotnet run -- <option>` from that folder.
