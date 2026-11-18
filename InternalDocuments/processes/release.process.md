# Release

Publishing a GitHub release triggers `.github/workflows/publish.yml`, which pushes to NuGet and attaches the standalone binaries.

## Steps

1. Push `master`.
1. Pick the next version: bump the patch (`v0.1.8` → `v0.1.9`) unless told otherwise.
1. List the user-facing commits: `git log --oneline <last tag>..master`, skipping refactors, tests, and environment changes.
1. Write the title and notes (below), and show them to the user before publishing.
1. Run `gh release create <tag> --target master --title "<title>" --notes-file <notes>`.
1. Watch the publish workflow until it succeeds.
1. NuGet takes 10–30 minutes to index the new version; say so.

## Title

`<tag> – <the headline change, in a few words>`, for example `v0.1.8 – Status, install, and update checks`.

## Notes

- Plain English, for users, not developers.
- Sections as needed: `## What's new`, `## Fixes`, `## Docs`.
- One bullet per change, saying what the user gets.
- End with `**Full Changelog**: https://github.com/LearnWithLlew/AiAttentionDashboard/compare/<previous tag>...<tag>`.

Fix an existing release with `gh release edit <tag> --title "<title>" --notes-file <notes>`.
