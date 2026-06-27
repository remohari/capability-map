# DigiDude Constitution

## Core Principles

### I. Spec-First Development

Every feature MUST begin with a written specification (`spec.md`) that is
reviewed and approved before any implementation work starts. The specification
is the single source of truth for scope, acceptance criteria, and success
metrics.

- Scope changes discovered during implementation MUST be reflected in the spec
  before the feature is considered complete.
- Ambiguous requirements MUST be resolved via `/speckit.clarify` before
  planning begins.

### II. Test-First (NON-NEGOTIABLE)

Tests MUST be written and confirmed to fail before the corresponding
implementation is written (Red-Green-Refactor).

- Backend Tests are written in Xunit
- Unit tests are REQUIRED for all non-trivial business logic (frontend
  utilities, Express route handlers, Prisma service methods).
- Integration tests are REQUIRED for Express API endpoints and Prisma
  database interactions.
- A task is NOT complete until its associated tests pass.
- Skipping or disabling tests requires explicit written justification in the
  PR description.

### III. Simplicity & YAGNI

The simplest solution that satisfies current requirements MUST be preferred
over clever or forward-looking designs.

- No feature, abstraction, or infrastructure MUST be added speculatively.
- Complexity beyond what is needed now MUST be justified in the plan's
  Complexity Tracking table.
- "We might need this later" is not a sufficient justification.

### IV. Observability & Logging

All production code paths MUST produce structured, actionable log output.

- Errors MUST be logged with enough context to reproduce the issue without a
  debugger (include request ID, user context, and stack trace where applicable).
- Business-critical events (user actions, state transitions, external DB calls)
  MUST emit structured log entries on the Express backend.
- Frontend errors MUST be surfaced to the user via appropriate UI feedback
  (toast / error boundary) and MUST NOT fail silently.
- Log levels MUST follow: DEBUG (dev-only), INFO (normal ops), WARN
  (recoverable), ERROR (requires attention).

### V. Versioning & Breaking Changes

Public REST API contracts MUST follow Semantic Versioning (MAJOR.MINOR.PATCH).

- MAJOR: backward-incompatible changes to any REST endpoint or Prisma schema
  that break existing clients.
- MINOR: backward-compatible new endpoints or additive schema migrations.
- PATCH: backward-compatible bug fixes and clarifications.
- Breaking Prisma migrations MUST include a rollback script and MUST NOT be
  merged without a documented deprecation period (minimum one release cycle).

### VI. OWASP Top 10 Compliance (NON-NEGOTIABLE)

Every feature MUST be reviewed against the OWASP Top 10 (2021) before merge.
The Constitution Check section in `plan.md` MUST include an explicit OWASP row
listing how each relevant item is addressed (or noting "n/a" with rationale).

Active controls per item — these are the project's baseline; new specs MUST NOT
weaken them and MUST extend them where the feature surface demands:

- **A01 Broken Access Control**: every customer-scoped endpoint requires
  `requireAuth` plus `requireRole(...)` or `requireCustomerAccess()`. KUNDE-Filter
  MUST happen serverseitig (DSGVO).
- **A02 Cryptographic Failures**: secrets in DB MUST be AES-256-GCM-encrypted via
  `crypto.service` (unique IV + AuthTag). `AI_ENCRYPTION_KEY` is environment-only,
  never committed. HTTPS-only in production.
- **A03 Injection**: Prisma ORM only — no raw SQL. Inputs MUST be validated with
  Zod. `dangerouslySetInnerHTML` MUST NOT be used.
- **A04 Insecure Design**: rate limiting MUST be enabled (global baseline +
  strict on auth). Body size limits MUST be set (5 MB JSON cap).
- **A05 Security Misconfiguration**: `helmet()` always-on, CORS allowlist,
  global error handler MUST NOT leak stack traces to clients.
- **A06 Vulnerable Components**: `npm audit` MUST report zero vulnerabilities
  (high/moderate/low) before each merge. CI/PR-Gate.
- **A07 Authentication Failures**: Auth0 only. Auth events logged INFO/WARN.
  No own session cookies (JWT-Bearer only).
- **A08 Software & Data Integrity**: lockfiles + Prisma migrations committed.
  No unbundled CDN-loaded scripts in the frontend.
- **A09 Logging & Monitoring Failures**: structured logs for every business event
  (assigned/unassigned/access_denied/etc.). Niemals PII / Tokens / E-Mails in Logs
  ausser pseudonymisiert (Domain-only).
- **A10 SSRF**: every fetch of user-supplied URLs MUST be guarded by
  `isPrivateUrl()` in `enrichment.service` (or equivalent). Block 127.x, 10.x,
  172.16-31.x, 192.168.x, ::1, 169.254.x (Cloud-Metadata-Endpoint).

Amendments to this list require a Constitution version bump (MINOR for additions,
MAJOR for relaxations).

## Technology Stack

DigiDude is a **single-page application**. The following tools are canonical
and MUST be used unless an amendment explicitly replaces them.

| Layer | Technology |
|---|---|
| Frontend framework | Angular + Vite + TypeScript |
| UI / Styling | Clarity |
| Backend | .net10 |
| Database ORM | PostgreSQL + EntityFramework|
| Package manager | npm (no yarn/pnpm without amendment) |
| Linting | ESLint + Prettier |
| Testing | Xunit |
| Run | Docker | 

**Constraints**:
- All new npm dependencies MUST be justified in the PR description; prefer
  packages already in the ecosystem over adding new ones.
- Prisma schema changes MUST be accompanied by a migration file (`prisma
  migrate dev`) committed in the same PR.
- Mantine components MUST be used for common UI patterns before writing
  custom components from scratch. Theme tokens (colors, spacing, radius,
  typography) MUST be sourced from `frontend/src/theme.ts`; hard-coded
  hex/rgba values in components are prohibited.
- Tailwind CSS and shadcn/ui MUST NOT be reintroduced without an amendment.

## Quality Gates

All pull requests MUST satisfy the following gates before merge:

1. **Spec gate** — a merged or approved `spec.md` exists for the feature.
2. **Lint gate** — `npx eslint . && npx prettier --check .` passes with zero
   errors.
3. **Test gate** — `npx vitest run` passes; no regressions introduced.
4. **Complexity gate** — any Complexity Tracking violations are documented
   and acknowledged by a reviewer.
5. **Observability gate** — new backend code paths include structured logging;
   new frontend error paths surface user-facing feedback.
6. **Migration gate** — any Prisma schema change includes a committed
   migration file and a rollback script.
7. **Versioning gate** — any API contract changes are correctly versioned and
   documented.
8. **Security gate (OWASP Top 10)** — `npm audit` reports zero vulnerabilities,
   AND the `plan.md` Constitution Check includes an OWASP row that documents
   each applicable item (A01–A10). Either explicit control reference or
   "n/a" with rationale. New routes that fetch user-supplied URLs MUST use the
   `isPrivateUrl()` guard (A10). New routes that accept user input MUST use Zod
   validation (A03). New routes touching customer-scoped data MUST use
   `requireCustomerAccess()` for reads, `requireRole('ADMIN','BERATER')` for writes
   (A01).

## Development Workflow

1. Create a feature branch following the naming convention
   `###-short-description` (sequential branch numbering enabled).
2. Run `/speckit.specify` → `/speckit.clarify` → `/speckit.plan` →
   `/speckit.tasks` before writing any code.
3. Implement tasks in the order defined by `tasks.md`, committing after each
   logical checkpoint.
4. Open a pull request referencing the spec and plan documents.
5. At least one reviewer MUST verify all Quality Gates before approving.
6. Squash-merge to `main` after approval; delete the feature branch.

## Governance

This constitution supersedes all other development practices and informal
conventions for the DigiDude project. In case of conflict, this document
takes precedence.

**Amendment procedure**:
1. Open a pull request editing `.specify/memory/constitution.md`.
2. State the proposed change, motivation, and version bump type
   (MAJOR / MINOR / PATCH) in the PR description.
3. At least one reviewer MUST approve. (Solo-Setup: Self-Review im PR-Body explizit dokumentieren.)
4. Run `/speckit.constitution` after the amendment is merged to propagate
   changes to dependent templates.
5. Document the change in the Sync Impact Report (HTML comment at top of
   this file).

**Versioning policy**: Semantic versioning applies to the constitution itself.
Use PATCH for wording fixes, MINOR for new principles or sections, MAJOR for
removal or incompatible redefinition of principles.

**Compliance review**: Constitution compliance MUST be verified during every
PR review via the Constitution Check section in `plan.md`. Violations require
explicit justification in the Complexity Tracking table.

**Version**: 1.3.0 | **Ratified**: 2026-03-24 | **Last Amended**: 2026-04-16