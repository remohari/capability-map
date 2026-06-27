# Implementation Plan: Rollenkonzept

**Branch**: `001-rollenkonzept` | **Date**: 2026-06-27 | **Spec**: [spec.md](/C:/AgenticDays/capability-map/specs/001-rollenkonzept/spec.md)
**Input**: Feature specification from `/specs/001-rollenkonzept/spec.md`

**Note**: This plan assumes implementation in the product application stack defined by the constitution, while this repository remains the documentation source of truth.

## Summary

Implement a role-based access control foundation for the four business roles `Kund:in`, `Berater:in`, `Bewerter:in`, and `Admin`. The implementation should add a single-role assignment model, enforce role-aware access checks on backend endpoints and frontend views, expose a maintainable rights matrix, and provide audit-friendly role change logging plus clear user-facing feedback on access denial.

## Technical Context

**Language/Version**: TypeScript for Angular frontend, .NET 10 for backend  
**Primary Dependencies**: Angular, Vite, TypeScript, Clarity, ASP.NET Core, Entity Framework, Auth0  
**Storage**: PostgreSQL via Entity Framework  
**Testing**: Xunit for backend business logic and integration coverage; frontend test suite to be aligned with the application repository standard during implementation  
**Target Platform**: Web SPA with backend API, deployed via Docker  
**Project Type**: Web application with Angular frontend and .NET backend  
**Performance Goals**: Authorization checks must not create a noticeable delay in normal page loads or protected API interactions; denied access responses should return within the same normal request budget as authorized reads  
**Constraints**: Single active role per user in v1; OWASP A01 enforcement on every protected route; structured logging for role assignment and access denial events; no speculative permission model beyond the four roles  
**Scale/Scope**: Four initial roles, all priority business functions mapped to one of these roles, and role enforcement across admin, customer, advisor, and evaluator flows

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

| Gate | Status | Plan Response |
|------|--------|---------------|
| Spec-First Development | PASS | Approved source spec exists in `specs/001-rollenkonzept/spec.md`; plan and follow-up artifacts derive from it. |
| Test-First | PASS | Tasks must create failing backend authorization and role-assignment tests before implementation; frontend access-state tests follow before UI changes. |
| Simplicity & YAGNI | PASS | Plan keeps a single active role per user and a fixed four-role matrix for v1; no multi-role or policy engine is introduced. |
| Observability & Logging | PASS | Role assignment, role change, and access denied events will emit structured backend logs; frontend access denial states must surface clear UI feedback. |
| Versioning & Breaking Changes | PASS | Any API contract changes for role assignment or protected resource behavior must be documented and versioned if externally visible. |
| OWASP A01 Broken Access Control | PASS | Protected routes must require authentication plus explicit role checks; customer-scoped reads remain server-side filtered. |
| OWASP A02 Cryptographic Failures | PASS | No new secret handling is introduced; existing auth and transport controls remain unchanged. |
| OWASP A03 Injection | PASS | Any new backend inputs for role updates or role-filtered queries must use validated request models; ORM access remains parameterized through Entity Framework. |
| OWASP A04 Insecure Design | PASS | Access denial defaults to reject; users without a valid role receive no protected access. |
| OWASP A05 Security Misconfiguration | PASS | Existing global security middleware remains in place; plan adds no bypass path around central auth or error handling. |
| OWASP A06 Vulnerable Components | PASS | No new dependency is planned; existing audit gate still applies before merge. |
| OWASP A07 Authentication Failures | PASS | Auth0 remains the only authentication mechanism; feature extends authorization only. |
| OWASP A08 Software & Data Integrity | PASS | Any model/schema change and lockfile updates must be committed together in the implementation repository. |
| OWASP A09 Logging & Monitoring Failures | PASS | Denied access and role state changes are explicit business events and must be logged without raw PII or tokens. |
| OWASP A10 SSRF | PASS | Feature does not introduce user-supplied outbound fetch behavior. |

## Project Structure

### Documentation (this feature)

```text
specs/001-rollenkonzept/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── authorization-contract.md
└── tasks.md
```

### Source Code (implementation target)

```text
backend/
├── src/
│   ├── Api/
│   ├── Application/
│   ├── Domain/
│   ├── Infrastructure/
│   └── Security/
└── tests/
    ├── Unit/
    └── Integration/

frontend/
├── src/
│   ├── app/
│   │   ├── core/
│   │   ├── features/
│   │   └── shared/
│   └── theme/
└── tests/
```

**Structure Decision**: The feature targets the constitution-defined SPA architecture with Angular frontend and .NET backend. This repository stores the planning artifacts only; implementation should follow the split `frontend/` and `backend/` structure above in the application repository.

## Phase 0 Research

- Confirm the minimal role-assignment model: one active role per user for v1.
- Define the recommended placement of role definitions, route guards, and authorization policies in the .NET backend.
- Define the recommended frontend strategy for role-aware navigation, route protection, and access-denied feedback in Angular.
- Identify the minimum audit logging payload for role changes and denied-access events that satisfies observability without leaking PII.
- Decide whether the initial rights matrix should be represented as code configuration, database seed data, or both.

## Phase 1 Design Outputs

- `research.md`: implementation decisions and rejected alternatives for the authorization model.
- `data-model.md`: role, user, role assignment, access rule, and audit event shapes plus validation rules.
- `contracts/authorization-contract.md`: backend and frontend contract for role assignment, authorization evaluation, denied-access behavior, and logging expectations.
- `quickstart.md`: delivery order for test-first implementation and verification.

## Phase 2 Implementation Direction

1. Add or update the domain model for roles and the single active role assignment.
2. Add failing backend unit and integration tests for role assignment, protected-route enforcement, and denied-access behavior.
3. Implement backend authorization policies, route enforcement, and structured event logging.
4. Add frontend role-aware navigation, protected route handling, and access-denied messaging with tests.
5. Validate the rights matrix against each priority user story and edge case.

## Complexity Tracking

No constitution violations are currently expected. This feature should remain within the simplest RBAC model that satisfies the approved spec.
