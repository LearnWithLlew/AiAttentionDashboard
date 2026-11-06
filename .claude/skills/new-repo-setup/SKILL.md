---
name: new-repo-setup
description: Use this skill whenever initializing a brand-new git repository, or when the user asks to set up/scaffold a new repo or project. Adds standard baseline files (.gitignore, LICENSE, .gitattributes) so every repo starts with consistent, predictable git behavior. Trigger on phrases like "new repo", "init this project", "set up a new repository", "scaffold a repo", even if the user only asks for one of the pieces (e.g. just a .gitignore) — mention the others are available too.
---

# New Repo Setup

Not every repo is buildable/CI-able — that's a separate concern (a separate skill), so this one stays scoped to git-level hygiene only.

## 1. `.gitignore`

Follow the `gitignore` skill.

## 2. `LICENSE`

**Always ask the user which license they want before adding one** 

- don't default a license silently. 
- when they answer, copy the verbatim license texxt from a reliable source


## 3. `.gitattributes`

Follow the `gitattributes` skill.
