# Plan

## Technical Approach

- Create `tools/import-claude-commands.ps1` to:
  - enumerate source `.md` files
  - parse optional YAML frontmatter
  - derive skill names from file names
  - build Codex `SKILL.md` files with normalized frontmatter
  - write output into `skills/<skill-name>/SKILL.md`
- Use repo-local `skills/` as the output location because the user asked to create skills in this project.

## Design Decisions

- Keep each imported skill minimal: only `SKILL.md` is required.
- Preserve source instruction bodies instead of attempting semantic rewrites during import.
- Prefer deterministic regeneration over hand-maintained derived files.

## Validation

- Run the import script.
- Verify the generated folder count matches the source command count.
- Inspect a sample of generated skills to confirm frontmatter normalization and body preservation.
