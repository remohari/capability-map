# Research: Rollenkonzept

## Decision 1: Use a single active role per user in v1

**Decision**: Each user receives exactly one active business role in the first implementation.

**Rationale**: The source Jira ticket only requires four roles and does not indicate mixed-role workflows. A single-role model keeps authorization checks testable, simplifies UI behavior, and avoids early ambiguity in reporting, support, and audit trails.

**Alternatives considered**:

- Multiple concurrent roles per user: rejected because it complicates precedence, UI presentation, and acceptance testing without evidence of immediate need.
- Attribute-based policy engine: rejected because it adds unnecessary abstraction for a four-role initial release.

## Decision 2: Enforce authorization on the backend as the source of truth

**Decision**: Backend route and business-operation checks are authoritative; frontend checks improve usability but do not grant access.

**Rationale**: This aligns with the constitution's A01 control and ensures customer-scoped data remains protected even if the frontend state is stale or manipulated.

**Alternatives considered**:

- Frontend-only feature hiding: rejected because it does not provide real access control.
- Database-only filtering without explicit policy enforcement: rejected because behavior becomes fragmented and harder to audit.

## Decision 3: Represent the initial rights matrix as explicit code configuration backed by tests

**Decision**: The first version should store role definitions and allowed capability mappings in explicit application configuration or policy code, with automated tests covering each protected path.

**Rationale**: The model is small and stable in v1. Code-based configuration is easier to review, diff, and test than a dynamic policy editor introduced too early.

**Alternatives considered**:

- Database-managed role matrix from day one: rejected because it adds administration and migration complexity before there is evidence that non-developers must edit the matrix.
- Hard-coded checks spread across handlers and components: rejected because it becomes brittle and obscures the source of truth.

## Decision 4: Log role changes and denied-access events as structured business events

**Decision**: The backend should emit structured logs for role assigned, role changed, role removed, and access denied events.

**Rationale**: These are business-critical state transitions and security-relevant events. Logging them consistently supports supportability, incident review, and compliance with the observability and OWASP requirements.

**Alternatives considered**:

- Only log unexpected exceptions: rejected because normal denied-access behavior and role changes still require audit visibility.
- Log full user identifiers or raw personal data: rejected because it creates unnecessary privacy exposure.

## Decision 5: Surface denied access explicitly in the UI

**Decision**: The frontend should render a clear access-denied state or redirect with contextual feedback whenever a user reaches a protected route or action they cannot use.

**Rationale**: Silent failure would violate the spec and the constitution's requirement that frontend error paths provide user-facing feedback.

**Alternatives considered**:

- Hide everything without explanation: rejected because it confuses users and support staff.
- Generic technical error pages: rejected because they do not explain a permission problem in user language.
