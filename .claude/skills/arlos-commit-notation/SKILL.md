---
name: arlos-commit-notation
description: Use whenever writing a commit message in a project that uses Arlo's Commit Notation (ACN), or when asked about commit prefixes like ". r", "^ F", "! B", or "@ D". Covers choosing the risk level and intention for the prefix.
---

# Arlo's Commit Notation

Arlo's Commit Notation puts the commit's risk level and intention in the first three characters of the summary line, so a reader can scan a log and see which commits are safe and what each one was trying to do.

Reference: https://github.com/RefactoringCombos/ArlosCommitNotation

## Format

```text
<risk> <intention> <summary>
```

Examples:

```text
. r Extract method ParseHeader
^ F Show retry count on the status page
! B Handle empty config file
@ D Draft notes on the release process
```

## Risk levels

| Code | Level | Meaning |
|---|---|---|
| `.` | Proven safe | Addresses known and unknown risks. |
| `^` | Validated | Addresses all known risks, verified by tests or similar checks. |
| `!` | Risky | Only the intended change is verified; some known risks are not. |
| `@` | Probably broken | No risk attestation; may not compile, e.g. a savepoint. |

A commit should contain at most one intended behavior change.
A commit with more than one is `@`.

## Intentions

| Code | Intention | Meaning |
|---|---|---|
| `F` / `f` | Feature | Change or extend one behavior without altering others. |
| `B` / `b` | Bugfix | Repair one undesired behavior without altering others. |
| `R` / `r` | Refactoring | Change structure without changing behavior. |
| `D` / `d` | Documentation | Change team-facing material without changing behavior. |

Uppercase versus lowercase is a project convention.
The two common ones are: uppercase means an intended behavior change, or uppercase means a user-visible change.
Follow the project's convention; check recent `git log` to see it in use.
Projects may also add their own extension intentions.

## Choosing the risk level

Pick the level the evidence supports, not the level you hope for.
When in doubt, use the riskier level.

### Feature and bugfix

- `^`: at most 8 lines changed (tests included), the area was already unit tested, and the tests were updated to match the change.
  For a bugfix, the buggy behavior was captured in a test first, and someone who speaks for the customer agreed on the old and new behavior.
- `.`: meets `^`, and only developers use the feature.
- `!`: has unit tests for the new behavior, but doesn't meet `^` (e.g. more than 8 lines).
- `@`: no automated tests, or unfinished.

### Refactoring

- `.`: a provable refactoring (done by a trusted automated tool, or by compiler-verified scripted steps), or a test-supported procedural refactoring done entirely in test code.
- `^`: a single named refactoring done by following its published steps, running the full tests along the way, on well-tested code.
- `!`: a named refactoring done by hand, or without whole-project test coverage.
- `@`: restructured by editing code freely, even in small steps.

### Documentation

In source files (comments, user-facing text):

- `.`: compiled output is byte-identical.
- `^`: verified by tests, or the text only appears on a developer-only screen.
- `!`: verified only by compiling and launching.
- `@`: unverified.

In other files:

- `.`: developer-facing docs that don't change a process.
- `^`: changes that affect developers by altering how the project builds or a process.
- `!`: changes to an important process.
- `@`: process experiments meant to gather information.

## Keep commits small

Many small commits at low risk are better than one big risky commit.
If a change won't fit one intention at one risk level, split it, e.g. a `. r` commit to prepare the code, then a `^ F` commit for the feature.
