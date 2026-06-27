# Repository Guidelines

## Project Context

This repository follows a spec-first workflow inspired by `github/spec-kit`.
Treat each capability as a small product specification that can move from idea to plan to implementation tasks.

## Working Rules

1. Start significant work by reading the relevant document in `specs/`.
2. Keep reusable guidance in `.specify/memory/`.
3. Update templates in `.specify/templates/` only when the workflow itself changes.
4. Prefer small, reviewable changes and keep specs aligned with implementation.
5. Use `git worktree` for parallel implementation streams so separate tasks can run in isolated working directories without blocking each other.
6. Execute Speckit steps manually only; do not auto-run them.
7. Ask for confirmation before each Speckit step.
8. Track the current implementation status in each capability `tasks.md` (checkboxes plus a short status summary).

## Capability Workflow

1. Create or update a spec in `specs/<capability-name>/spec.md`.
2. Draft an execution plan in `specs/<capability-name>/plan.md`.
3. Break the work into `specs/<capability-name>/tasks.md`.
4. Implement and keep the documents current as decisions change.
