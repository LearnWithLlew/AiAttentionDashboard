---
name: gitignore
description: Use whenever creating or editing a .gitignore file, or when deciding how to keep local/generated files out of a git repo.
---

# .gitignore

The first line of every `.gitignore` must be:

```
.ignore/
```

Then add patterns for whatever the repo's languages and tools generate (build outputs, dependency folders, IDE/user-specific files, etc.).

Why: `.ignore/` gives every repo the same conventional place for local scratch files that should never be committed, so there's no need to invent a new ignore rule each time.

If you ever need a place in the repo to write a temporary or scratch file, prefer to put it under `.ignore/` so it won't be accidentally committed, rather than adding an new entry to the `.gitignore`.

When editing an existing `.gitignore`, add `.ignore/` as the first line if it's missing, and leave the rest of the file intact.
