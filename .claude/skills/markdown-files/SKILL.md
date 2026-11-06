---
name: markdown-files
description: Use whenever creating or editing a Markdown (.md) file, including READMEs, docs, and SKILL.md files.
---

# Markdown Files

- put one sentence per line in markdown files for better readability and version control.
- for numbered lists always use `1.` for every line. the rendered output will still show correct numbering.
- for code snippets, use Markdown Snippets from a test or production code. this way we know for certain it is valid. You can use `snippet:` for examples (which will add a link), and `include:` for bare content (which will include the raw contents directly). the latter is useful with a VerifyTest that generates a `.include.md` file.
