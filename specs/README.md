# Specs

Create one folder per capability:

- `specs/<capability-name>/spec.md`
- `specs/<capability-name>/plan.md`
- `specs/<capability-name>/tasks.md`

The expected workflow is:

1. Write or update `spec.md` first.
2. Resolve ambiguities before planning.
3. Create `plan.md` with Constitution Check, explicit OWASP A01-A10 review,
   observability notes, and semantic-versioning impact.
4. Create `tasks.md` with failing tests scheduled before implementation tasks.
5. Keep all three files current when decisions change during delivery.

Example:

- `specs/customer-onboarding/spec.md`
