# Capability Map

Capability Map is a spec-first repository scaffold with a project constitution in
`.specify/memory/constitution.md`. It is prepared for use with
[github/spec-kit](https://github.com/github/spec-kit), but the local workflow is
governed by the DigiDude constitution, not by upstream defaults.

## Purpose

Use this repository to define, plan, and track product or platform capabilities
through lightweight specifications, implementation plans, and task breakdowns
that remain aligned with the constitution.

## Layout

- `.specify/` holds project memory and reusable templates.
- `specs/` holds capability specifications.
- `AGENTS.md` gives Codex-specific working rules for this repository.
- `CLAUDE.md` gives the companion Claude-specific development guidance.
- `.specify/scripts/powershell/update-agent-context.ps1` can refresh both `AGENTS.md` and `CLAUDE.md` from current plan data.

## Workflow

1. Create or update a capability spec in `specs/<capability-name>/spec.md`.
2. Clarify ambiguities before planning.
3. Draft `plan.md` with Constitution Check, OWASP review, observability, and
   versioning impact.
4. Break the work into `tasks.md`, with tests scheduled before implementation.
5. Implement only after the above documents are in place and keep them current
   if scope changes.

## Next Steps

1. Create a new capability spec under `specs/`.
2. Use the updated templates in `.specify/templates/` for plan and task setup.
3. If you install the `specify` CLI locally, keep the generated artifacts aligned
   with `.specify/memory/constitution.md`.

## Baseline Verification Commands

- `dotnet build backend/CapabilityMap.Backend.csproj`
- `npm --prefix frontend run build`
- `npm --prefix frontend audit`
