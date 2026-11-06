---
name: subagent-models
description: Use whenever spawning a subagent (the Agent tool) or delegating work to another model, and when choosing which model a subagent should run on.
---

# Subagent Models

A subagent can run on a different model than the parent session.
Match the model to the task: use the cheapest model that can do the job well, and spend a stronger model only where the quality difference matters.

## Choosing a model

The Agent tool's `model` parameter accepts `haiku`, `sonnet`, `opus`, or `fable`.
If it is omitted, the subagent uses the agent definition's model, else the parent's model.

| Task | Model |
|---|---|
| Search, file lookups, listing, simple extraction, mechanical edits | `haiku` |
| Typical coding, focused refactors, summarizing, running and triaging tests | `sonnet` |
| Subtle bugs, design decisions, security review, independent second opinions | `opus` |
| The hardest problems, where a wrong answer is costly | `fable` |

When in doubt, start one tier lower than your instinct and move up if the result is thin or wrong.
Don't hard-code model versions in skills or prompts; the aliases above track the current release.

## Caveats

- `subagent_type: "fork"` always inherits the parent's context and model; a `model` override is ignored.
  To use a different model, start a fresh agent instead, and put everything it needs in the prompt.
- The `effort` parameter is only for when the user, or project instructions, ask for a specific effort level.
  Otherwise omit it.
- A fresh agent knows only what the prompt tells it, so a cheaper model needs a more explicit brief, not a shorter one.
  State the goal, the files to read, what has already been ruled out, and what form the answer should take.

## Review and verification

Use a different model from the one that wrote the work when you want an independent read.
Give the reviewer the code or diff, not your conclusion, so it isn't anchored on your answer.

## Parallel work

Independent subagents can be launched in the same message so they run concurrently.
Each one costs tokens, so only spawn what the task needs, and do small lookups directly instead of delegating them.
