---
name: skill-stub-request
description: Use this skill whenever the user writes a message starting with "skill:" followed by a short name or topic (e.g. "skill: CMD scripts"). This is shorthand for "create a placeholder skill for me to fill in later" — NOT a request to immediately research, interview, or fully build out that skill via skill-creator's full workflow. Trigger on this exact "skill: <topic>" pattern.
---

# Skill Stub Request

When the user says `skill: <topic>`, they mean: scaffold an empty, minimal skill under their AgentSkills collection (`C:\Code\AgentSkills\skills\<kebab-case-name>\`) so they can fill in the real content themselves later. Do not run the full skill-creator interview/draft/eval loop unless they explicitly ask for that.

## What to do

1. Pick a kebab-case name from the topic (e.g. "CMD scripts" → `cmd-scripts`).
2. Create `C:\Code\AgentSkills\skills\<name>\SKILL.md` with just a frontmatter stub and a TODO — no real instructions yet:

```markdown
---
name: <name>
description: TODO — describe what this skill does and when it should trigger.
---

# <Title>

TODO: fill in instructions for this skill.
```

3. Confirm briefly with the user that the stub was created and where, so they know it's waiting for them.

Why this exists: the user builds skills incrementally — they want a placeholder reserved the moment they think of one, without derailing into a full interview/build cycle right then.
