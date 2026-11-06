# AI Attention Dashboard

Use this dashboard to tell the user what needs their attention.
The user glances at it instead of reading the terminal.

## Workflow

1. Run `aiattentiondashboard --start`.
   It opens the dashboard in a browser and prints the path of the HTML file it shows.
1. Write the HTML file whenever something needs the user's attention, or when that changes.
1. Run `aiattentiondashboard --refresh` after every write so the browser shows it.
1. Run `aiattentiondashboard --stop` when the session is over.

## The window

The dashboard sits in a small browser window on the right side of the screen.
It is short and wide: about one fifth of the screen height.
Everything must fit without scrolling.

## What to show

- Show only high-level action items, not progress logs or details.
  The details belong in the terminal.
- Make the one thing you need from the user obvious: what to do and where (for example, "Decision needed in the terminal").
- Show a short status line for what you are working on when nothing is needed from the user.
- Remove items once they are handled.
  Stale items train the user to ignore the dashboard.

## How to write the HTML

- Write one complete, self-contained HTML file.
- Put CSS inline in a `<style>` element.
- Don't load external scripts, styles, fonts, or images.
- Use a large enough font to read at a glance, and few words.
- Overwrite the whole file each time rather than appending.
