---
name: bash-scripts
description: Use whenever creating or editing a Bash script, shell script, or extensionless script meant to run with bash, including the Bash half of a CMD-to-Bash entry point.
---

# Bash Scripts

Bash is acceptable for smallish scripts (say under 100 lines).
If a better language is readily available, prefer to use that language.

## Standard boilerplate

Every Bash script starts with:

```bash
#!/usr/bin/env bash
set -euo pipefail
```

- `#!/usr/bin/env bash` finds bash on the `PATH`, so it works on Linux, Mac, and Git for Windows.
- `set -e` exits on the first failing command.
- `set -u` treats unset variables as errors.
- `set -o pipefail` makes a pipeline fail if any command in it fails, not just the last one.

## File conventions

- if it's paired with a CMD script, use the same name without a file extension, so `./foo` works on PowerShell, Mac, and Linux.
- Mark the file executable in git, even on Windows: `git update-index --chmod=+x foo`. After commit, verify that this change took effect with `git ls-files --stage foo`.
- Use LF line endings; a CRLF shebang line fails with `bad interpreter`. See the gitattributes skill.

## Windows entry point

To make the script easy to run from CMD, pair it with a one-line CMD script of the same name, as described in the cmd-scripts skill.
