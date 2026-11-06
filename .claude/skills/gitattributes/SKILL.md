---
name: gitattributes
description: Use whenever creating or editing a .gitattributes file, or when dealing with line-ending (CRLF/LF) problems in a git repo.
---

# .gitattributes

Use exactly this content as the baseline, then adjust/add lines for any other file types in the repo that need explicit line-ending handling:

```
* -text
*.verified.* text eol=lf
*.cmd text eol=crlf
*.bat text eol=crlf
```

Why: `* -text` turns off Git's automatic text/binary detection and CRLF/LF normalization by default — avoids Git silently rewriting line endings on files it misclassifies. The two overrides below are important special cases: Verify-style snapshot files (`*.verified.*`, e.g. from Verify.NET) must stay LF so diffs don't spuriously flip on different OSes, while Windows batch scripts (`*.cmd`, `*.bat`) require CRLF to run correctly.
