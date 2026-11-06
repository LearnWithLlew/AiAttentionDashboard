---
name: committing
description: Use whenever making a git commit, writing a commit message, or deciding what to stage. Covers checking the change before committing, keeping commits focused, and writing the message.
---

# Committing

## Before committing

1. Run build-and-test (see the developer-experience skill) and make sure it passes.
1. Review what is staged with `git status` and `git diff --staged`.
1. Stage specific files by name rather than `git add -A`, so unrelated or scratch files don't sneak in.

Scratch files belong under `.ignore/` (see the gitignore skill), not in a commit.

## Keep commits focused

- One logical change per commit.
- If the working tree mixes unrelated changes, split them into separate commits.
- Don't mix refactoring with behavior changes in the same commit.

## Commit message

- First line: a short summary in the imperative mood ("Add X", not "Added X"), no trailing period.
- Use a prefixing system (e.g. Arlo's Commit Notation — see the arlos-commit-notation skill — or Conventional Commits). Each project is different in its conventions, so follow the project's chosen system.
- Leave a blank line, then a body explaining *why* the change was made, if that isn't obvious from the summary.
- If it fixes or relates to a bug report, mention it in the body of the commit message.
- Keep wording neutral; see the no-personal-reference skill.
- Don't push unless asked.
