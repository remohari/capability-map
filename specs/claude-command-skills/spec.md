# Claude Command Skills

## Goal

Create repo-local Codex skills from the Claude command definitions in `C:\AgenticDays\digiberatung\DigiDude\.claude\commands`.

## Scope

- Import each `.md` file in the source command folder into a separate skill folder under `skills/`.
- Normalize Claude command file names into Codex skill names using lowercase hyphen-case.
- Convert source frontmatter into Codex-compatible `SKILL.md` frontmatter with only:
  - `name`
  - `description`
- Preserve the command body as the main skill instructions.
- Keep the import process repeatable via a script in this repository.

## Functional Requirements

1. The repository must contain a repeatable import script for converting Claude command files into Codex skills.
2. The script must read every top-level `.md` file from the configured source folder.
3. Each imported command must produce a skill folder named after the normalized command file name.
4. Each skill folder must contain a `SKILL.md` file.
5. Each generated `SKILL.md` must contain valid YAML frontmatter with only `name` and `description`.
6. The generated skill body must preserve the source command content after its frontmatter block.
7. The import must overwrite previously generated skill files for the same command names to keep regeneration deterministic.

## Success Criteria

- All command files in the source folder are represented as skill folders in `skills/`.
- Each generated `SKILL.md` is readable and structurally valid for Codex skill discovery.
- Regenerating the import produces stable results without manual editing.
