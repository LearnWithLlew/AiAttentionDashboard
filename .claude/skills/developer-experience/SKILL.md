---
name: developer-experience
description: Use whenever setting up a repo, adding a build system or test framework, or when someone asks how to build or test a project. Every repo should have a single build-and-test entry point.
---

# Developer Experience

## Build-and-test

> For every source repository in the world,
> I should be able to clone it onto a clean machine,
> Run the "Build and Test" script in the root,
> and it just works.

This is aspirational; there will always be reasons it's not perfectly achivable, btu we strive for it anyway. 

## Additional notes:

- CI should run the same script as local development, ensuring consistency between local and CI environments.
- Two repos on the same machine should not interfere with each other's build or test processes.
- If there is a linter or static analysis, it should also run as part of build-and-test.
- There should also be a "tidy code" script that formats and applys lint autofixers.
- If the repo is not a library, there should also be a "run" script.
- Performance of build-and-test matters less than correctness
- If you ever want to run a subset (e.g. just run a single test) go ahead and do so directly. You don't have to use build-and-test every time.
