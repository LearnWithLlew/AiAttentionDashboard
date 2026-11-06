---
name: cmd-scripts
description: Use whenever creating or editing a Windows .bat/.cmd file, or asked for a batch script, CMD entry point, or Windows command-line wrapper.
---

# CMD Scripts

Windows Batch files aka CMD scripts are extremely difficult to get right, but they are very useful as entry points for humans. Therefore:

**Every CMD script should only be one line long, typicallying calling some other safer tool/language to do the work.**

This one line should be prefixed with `@call`. 

Some examples of acceptable one-line CMD scripts:

## C# single-line script

Put the useful behavior in a CSharp file and make an entry point CMD script that does:

```batch
@call dotnet run {path/tool/tool.cs} --verbosity quiet --nologo
```

(Requires dotnet 10).

## Mise tasks

Mise handles both tools and tasks, making it easy to create one-line CMD scripts that delegate work to Mise.

```batch
@call mise --quiet run %~n0
```

Requires Mise.

## Pass control to Bash

If you're in a Git repo, you have Bash available. Bash is not a good language but it is far better than Batch. Use some other language if one is readily availble, but Bash is acceptable for smallish scripts (say under 100 lines).

```batch
@call "C:\Program Files\Git\bin\bash.exe" %~dpn0 %*
```

paired with a corresponding Bash script in the same location. Note that the bash script should not have an extension, so that `./foo` will work on both PowerShell and Mac/Linux.

Requires Git for Windows.

