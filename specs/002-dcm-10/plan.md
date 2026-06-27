# Implementation Plan: Bereich/Kunde anlegen und kundenbezogene Zugriffsrechte

**Branch**: `002-dcm-10` | **Date**: 2026-06-27 | **Spec**: [spec.md](/C:/AgenticDays/capability-map/specs/002-dcm-10/spec.md)
**Input**: Feature specification from `/specs/002-dcm-10/spec.md`

**Note**: This plan assumes implementation in the product application stack defined by the constitution, while this repository remains the documentation source of truth.

## Summary

Implement a customer area management system that allows authorized administrators to create new business areas with associated customer contexts, assign per-area access rights, and enforce strict customer-scoped access boundaries. The feature builds on the existing role-based access control foundation (001-rollenkonzept) by adding area-level authorization and customer isolation.

## Technical Context

**Language/Version**: TypeScript for Angular frontend, .NET 10 for backend  
**Primary Dependencies**: Angular, Vite, TypeScript, Clarity, ASP.NET Core, Entity Framework, Auth0  
**Storage**: PostgreSQL via Entity Framework  
**Testing**: Xunit for backend business logic and integration coverage; frontend test suite aligned with application repository standard  
**Target Platform**: Web SPA with backend API, deployed via Docker  
**Project Type**: Web application with Angular frontend and .NET backend  
**Performance Goals**: Area creation and authorization checks must not create a noticeable delay in normal page loads or protected API interactions; denied access responses should return within the same normal request budget as authorized reads  
**Constraints**: Single active role per user in v1; OWASP A01 enforcement on every protected route; structured logging for area creation, permission changes, and access denial events; no speculative permission model beyond the four roles; customer-scoped reads remain server-side filtered  
**Scale/Scope**: Initial areas per customer with separate permission sets; all priority business functions mapped to area-level authorization

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

| Gate | Status | Plan Response |
|------|--------|---------------|
| Spec-First Development | PASS | Approved source spec exists in `specs/002-dcm-10/spec.md`; plan and follow-up artifacts derive from it. |
| Test-First | PASS | Tasks must create failing backend area-creation and customer-access tests before implementation; frontend access-state tests follow before UI changes. |
| Simplicity & YAGNI | PASS | Plan keeps a single active role per user and a fixed four-role matrix for v1; no multi-role or policy engine is introduced. |
| Observability & Logging | PASS | Area creation, permission changes, and access denied events will emit structured backend logs; frontend access denial states must surface clear UI feedback. |
| Versioning & Breaking Changes | PASS | Any API contract changes for area creation or protected resource behavior must be documented and versioned if externally visible. |
| OWASP A01 Broken Access Control | PASS | Protected routes must require authentication plus explicit role checks; customer-scoped reads remain server-side filtered. |
| OWASP A02 Cryptographic Failures | PASS | No new secret handling is introduced; existing auth and transport controls remain unchanged. |
| OWASP A03 Injection | PASS | Any new backend inputs for area creation or permission management must use validated request models; ORM access remains parameterized through Entity Framework. |
| OWASP A04 Insecure Design | PASS | Access denial defaults to reject; users without a valid role or area permission receive no protected access. |
| OWASP A05 Security Misconfiguration | PASS | Existing global security middleware remains in place; plan adds no bypass path around central auth or error handling. |
| OWASP A06 Vulnerable Components | PASS | No new dependency is planned; existing audit gate still applies before merge. |
| OWASP A07 Authentication Failures | PASS | Auth0 remains the only authentication mechanism; feature extends authorization only. |
| OWASP A08 Software & Data Integrity | PASS | Any model/schema change and lockfile updates must be committed together in the implementation repository. |
| OWASP A09 Logging & Monitoring Failures | PASS | Denied access and area state changes are explicit business events and must be logged without raw PII or tokens. |
| OWASP A10 SSRF | PASS | Feature does not introduce user-supplied outbound fetch behavior. |

## Project Structure

### Documentation (this feature)

```text
specs/002-dcm-10/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── area-management-contract.md
└── tasks.md
```

### Source Code (implementation target)

```text
backend/
├── src/
│   ├── Api/
│   │   ├── Controllers/
│   │   └── Middleware/
│   ├── Application/
│   │   └── Security/
│   ├── Domain/
│   │   └── Security/
│   ├── Infrastructure/
│   │   ├── Logging/
│   │   └── Persistence/
│   └── Security/
└── tests/
    ├── Unit/
    └── Integration/

frontend/
├── src/
│   ├── app/
│   │   ├── core/
│   │   │   ├── auth/
│   │   │   └── navigation/
│   │   ├── features/
│   │   │   └── admin/
│   │   └── shared/
│   │       └── access-denied/
│   └── theme/
└── tests/
```

**Structure Decision**: The feature targets the constitution-defined SPA architecture with Angular frontend and .NET backend. This repository stores the planning artifacts only; implementation should follow the split `frontend/` and `backend/` structure above in the application repository.

## Phase 0 Research

- Confirm the minimal area creation model: a new area is always associated with exactly one customer context.
- Define the recommended placement of area management endpoints, customer-scope enforcement, and permission management in the .NET backend.
- Define the recommended frontend strategy for area creation forms, permission matrices, and access-denied feedback in Angular.
- Identify the minimum audit logging payload for area creation, permission changes, and denied-access events that satisfies observability without leaking PII.
- Decide whether the initial area-permission mapping should be represented as code configuration, database seed data, or both.

## Phase 1 Design Outputs

- `research.md`: implementation decisions and rejected alternatives for the area management and customer-access model.
- `data-model.md`: area, customer context, area permission, and audit event shapes plus validation rules.
- `contracts/area-management-contract.md`: backend and frontend contract for area creation, permission management, customer-scope enforcement, and logging expectations.
- `quickstart.md`: delivery order for test-first implementation and verification.
