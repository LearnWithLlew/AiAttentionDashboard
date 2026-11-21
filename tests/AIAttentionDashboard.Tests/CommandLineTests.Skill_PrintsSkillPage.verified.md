# AI Attention Dashboard

Use this dashboard to tell the user what needs their attention.
The user glances at it instead of reading the terminal.

## Prime directive

Before you do anything, tell the dashboard what you are about to do.
The moment you finish, tell it again.
No exceptions, even for small or quick steps.

- Right before you start: "Working: what you are about to do".
- Right after you finish: "Done: what you finished", or "Waiting: the question, and where to answer".

The main agent sends these to the dashboard subagent and never talks about the dashboard to the user.
The subagent keeps the page current.

## Delegate to a subagent

Keeping the dashboard current is easy to forget while you work, so hand it to a subagent.

### If you are the main agent

1. Run `aiattentiondashboard --start`.
1. Start a background subagent on a small, fast model (in Claude Code, Haiku).
   Tell it: "You own the AI Attention Dashboard. Run `aiattentiondashboard --skill` and follow it as the dashboard subagent. I'll message you each time my state changes."
1. Follow the prime directive: send it a one-line message right before you start anything, and right after you finish.
   Include checklist progress when you have a checklist.
1. Don't mention the dashboard or its status to the user; that's the subagent's job.
1. Run `aiattentiondashboard --stop` when the session is over.

If you can't start subagents, follow the workflow below yourself.

### If you are the dashboard subagent

- The main agent already started the dashboard, so skip `--start` and follow the rest of the workflow and this page.
- Turn each message into the page: set the state, show what matters, and remove anything stale.
- Never ask the user anything; you only show what the main agent tells you.
- Reply to the main agent with one short line, so its context stays small.
- Leave `--stop` to the main agent.

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
Run `aiattentiondashboard --size` to get the page's current width and height, and design for that.

## What to show

- Show only high-level action items, not progress logs or details.
  The details belong in the terminal.
- Make the one thing you need from the user obvious, and say where to respond.
- Show a short status line for what you are working on when nothing is needed from the user.
- Remove items once they are handled.
  Stale items train the user to ignore the dashboard.

## Show your state

Always show which state you are in, so the user can tell at a glance without reading:

| State | When | Color | Icon |
| - | - | - | - |
| Working | You are busy and the user doesn't need to act. | Amber `#d18a00` | `…` |
| Waiting | You need input from the user. | Red `#c62828` | `?` |
| Done | You have finished, or are idle. | Green `#1e7a3c` | `✓` |

- Put the state in a narrow colored bar down the left edge, with its icon centered in it.
- Keep the rest of the page dark and neutral, so the state color is what stands out.

## How to write the HTML

- Write one complete, self-contained HTML file.
- Put CSS inline in a `<style>` element.
- Don't load external scripts, styles, fonts, or images.
- Use a large enough font to read at a glance, and few words.
- Overwrite the whole file each time rather than appending.


## Guidance 

### Questions
If possible, make a very short version of the question so that the user can keep that context in mind when they go back to the terminal.
Otherwise, use a generic "See the terminal." as the question.

Lay out a question from top to bottom like this:

1. `Question (terminal):` on a line of its own in red, naming where to answer.
1. The short version of the question.

### Multiple Choice Questions
When there is only 1 question, but it multiple choice it would be nice to summarize the question & those choices in the dashboard.
Only do this if you can make it short enough the you can phrase it to be taken in at a glance.
If there are numbers or letters for each option, show them so it is easy for the use to shift back to the terminal
