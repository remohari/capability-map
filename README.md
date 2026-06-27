# Capability Map

Capability Map is a spec-driven repository scaffold prepared for use with [github/spec-kit](https://github.com/github/spec-kit).

## Purpose

Use this repository to define, plan, and track product or platform capabilities through lightweight specifications.

## Layout

- `.specify/` holds project memory and reusable templates.
- `specs/` holds capability specifications.
- `AGENTS.md` gives Codex-specific working rules for this repository.

## Next Steps

1. Create a new capability spec under `specs/`.
2. Refine the constitution in `.specify/memory/constitution.md`.
3. If you install the `specify` CLI locally, run `specify init --here --ai codex --force` to layer in the upstream automation scripts without changing the repo intent.
