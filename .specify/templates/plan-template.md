# Implementation Plan: [FEATURE]

**Branch**: `[###-short-description]` | **Date**: [DATE] | **Spec**: [link]
**Input**: Feature specification from `/specs/[###-short-description]/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/plan-template.md` for the execution workflow.

## Summary

[Extract from feature spec: primary requirement + technical approach from research]

## Technical Context

<!--
  ACTION REQUIRED: Replace the content in this section with the technical details
  for the project. The structure here is presented in advisory capacity to guide
  the iteration process.
-->

**Language/Version**: [.NET 10 backend, Angular + Vite + TypeScript frontend or NEEDS CLARIFICATION]  
**Primary Dependencies**: [ASP.NET Core, Entity Framework, Clarity, Auth0, Zod or NEEDS CLARIFICATION]  
**Storage**: [PostgreSQL or N/A]  
**Testing**: [Xunit for backend, frontend test approach if applicable]  
**Target Platform**: [Dockerized SPA + API]
**Project Type**: [single-page application with backend API]  
**Performance Goals**: [domain-specific, e.g., 1000 req/s, 10k lines/sec, 60 fps or NEEDS CLARIFICATION]  
**Constraints**: [Spec-first, test-first, structured logging, OWASP review, semver for public REST APIs]  
**Scale/Scope**: [domain-specific, e.g., 10k users, 1M LOC, 50 screens or NEEDS CLARIFICATION]

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

| Gate | Status | Notes |
|------|--------|-------|
| Spec-first | [PASS/FAIL] | `spec.md` exists and unresolved ambiguities are clarified before planning |
| Test-first | [PASS/FAIL] | Tests to be written first and confirmed failing before implementation |
| Simplicity | [PASS/FAIL] | Any non-trivial complexity justified in Complexity Tracking |
| Observability | [PASS/FAIL] | Backend structured logs and frontend user-facing error handling identified |
| Versioning | [PASS/FAIL] | REST/API or schema changes classified as MAJOR/MINOR/PATCH |
| OWASP A01-A10 | [PASS/FAIL] | Explicit row below covers every relevant item or marks `n/a` with rationale |

### OWASP Top 10 Review

| Item | Applicability | Planned Control / Rationale |
|------|---------------|-----------------------------|
| A01 Broken Access Control | [applicable/n/a] | [e.g., `requireAuth` + role/customer access guard] |
| A02 Cryptographic Failures | [applicable/n/a] | [e.g., AES-256-GCM via crypto service, env-only keys] |
| A03 Injection | [applicable/n/a] | [e.g., Entity Framework only, Zod validation, no dangerous HTML injection] |
| A04 Insecure Design | [applicable/n/a] | [e.g., rate limiting, body size limits] |
| A05 Security Misconfiguration | [applicable/n/a] | [e.g., helmet-equivalent headers, CORS allowlist, safe error handling] |
| A06 Vulnerable Components | [applicable/n/a] | [e.g., `npm audit` clean before merge] |
| A07 Authentication Failures | [applicable/n/a] | [e.g., Auth0 only, auth events logged] |
| A08 Software & Data Integrity | [applicable/n/a] | [e.g., lockfiles committed, no CDN scripts] |
| A09 Logging & Monitoring Failures | [applicable/n/a] | [e.g., structured business-event logs, no raw PII/tokens in logs] |
| A10 SSRF | [applicable/n/a] | [e.g., `isPrivateUrl()` or equivalent for user-supplied URLs] |

## Project Structure

### Documentation (this feature)

```text
specs/[###-short-description]/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── data-model.md        # Phase 1 output (/speckit.plan command)
├── quickstart.md        # Phase 1 output (/speckit.plan command)
├── contracts/           # Phase 1 output (/speckit.plan command)
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)
<!--
  ACTION REQUIRED: Replace the placeholder tree below with the concrete layout
  for this feature. Delete unused options and expand the chosen structure with
  real paths (e.g., apps/admin, packages/something). The delivered plan must
  not include Option labels.
-->

```text
backend/
├── src/
│   ├── Api/
│   ├── Domain/
│   ├── Infrastructure/
│   └── Security/
└── tests/

frontend/
├── src/
│   └── app/
└── [tests if applicable]
```

**Structure Decision**: [Document the selected structure and reference the real
directories captured above]

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| [e.g., 4th project] | [current need] | [why 3 projects insufficient] |
| [e.g., Repository pattern] | [specific problem] | [why direct DB access insufficient] |
