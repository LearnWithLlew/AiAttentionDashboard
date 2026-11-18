# --install

`aiattentiondashboard --install` takes no arguments.
It doesn't install anything itself.
It prints instructions that let your AI agent install the `/aiattentiondashboard` command, in whatever way that agent does it.

## What it prints

1. Ask the user: install for this directory only, or globally?
1. Install a command or skill named `aiattentiondashboard`, the way your agent does it.
1. Its contents: the startup process.

## Where the startup process comes from

`InternalDocuments/processes/aiattentiondashboard.startup.process.md` is embedded when the tool is built.
Edit that file, and the next release ships the change.
