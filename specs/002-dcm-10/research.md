# Research: Bereich/Kunde anlegen und kundenbezogene Zugriffsrechte

## Decision 1: Area creation requires a valid customer context

**Decision**: Every new area must be associated with exactly one customer context at creation time. Customer context data (name, identifier, required fields) must be validated before the area is persisted.

**Rationale**: The spec requires that an area is always tied to a customer context (FR-002). Requiring valid customer data at creation time ensures referential integrity and prevents orphaned areas.

**Alternatives considered**:

- Allow areas without a customer context initially: rejected because FR-002 mandates a clear customer association, and deferred assignment would complicate access control.
- Allow duplicate customer contexts for the same customer: rejected because it creates ambiguity in access control and reporting.

## Decision 2: Per-area access rights are separate from role-based rights

**Decision**: Area permissions are stored and evaluated independently from the existing role definitions. A user must have both a valid role AND an explicit area permission to access a specific area.

**Rationale**: The spec requires that area permissions are separate and do not grant access to other areas (FR-004, FR-006). This separation allows granular control per area while keeping the role model stable.

**Alternatives considered**:

- Extend the role model with area assignments: rejected because it conflates role and scope, making it harder to manage per-area permissions.
- Use a shared policy engine for both role and area checks: rejected because it adds unnecessary complexity for v1.

## Decision 3: Customer-scoped access is enforced server-side only

**Decision**: Backend endpoints must filter and enforce customer scope; frontend navigation can hide or disable restricted items for usability but does not provide real access control.

**Rationale**: This aligns with the constitution's A01 control and Decision 2 from the 001-rollenkonzept research. Customer data isolation must be enforced at the server level regardless of frontend state.

**Alternatives considered**:

- Frontend-only scope filtering: rejected because it does not provide real protection.
- Database-level row-level security without application checks: rejected because it fragments authorization logic and makes auditing harder.

## Decision 4: Area permission changes are logged as structured audit events

**Decision**: The backend must emit structured logs for area creation, permission assignment, permission changes, and access denial events.

**Rationale**: These are business-critical state transitions and security-relevant events. Logging them consistently supports supportability, incident review, and compliance with the observability and OWASP requirements.

**Alternatives considered**:

- Only log unexpected exceptions: rejected because normal permission changes and denied access require audit visibility.
- Log full user identifiers or raw personal data: rejected because it creates unnecessary privacy exposure.

## Decision 5: Duplicate area detection prevents accidental double-creation

**Decision**: When creating a new area, the system must check for existing areas with the same customer context and key identifiers. If a duplicate is detected, the creation must be rejected with a clear message.

**Rationale**: The spec's edge cases explicitly ask about duplicate area creation. Preventing duplicates ensures data integrity and avoids confusion in access control.

**Alternatives considered**:

- Allow duplicates and merge later: rejected because it creates ambiguity in permissions and reporting.
- Soft-delete duplicates: rejected because it adds complexity without clear benefit for v1.

## Decision 6: Areas without permissions deny all customer access

**Decision**: An area with no assigned permissions must default to denying all customer access. Only explicit permission grants allow access.

**Rationale**: The spec requires that areas without permissions must not allow customer access (FR-011). This follows the principle of least privilege and ensures that new areas do not inadvertently expose data.

**Alternatives considered**:

- Allow access if the user has a valid role: rejected because role alone does not imply area-specific authorization.
- Use a default allow policy: rejected because it violates the principle of least privilege.
