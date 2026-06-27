# Data Model: Rollenkonzept

## Entity: Role

**Purpose**: Defines one of the four supported business roles and the business capabilities attached to it.

**Fields**:

- `name`: one of `Kund:in`, `Berater:in`, `Bewerter:in`, `Admin`
- `description`: stakeholder-readable explanation of the role purpose
- `allowedAreas`: list of protected areas visible to the role
- `allowedActions`: list of actions the role may execute
- `isAssignable`: indicates whether admins may assign this role in normal operation

**Validation Rules**:

- Role name must be unique.
- Only the four approved roles are valid in v1.
- Every role must define at least one explicit allow or explicit exclusion boundary in the rights matrix.

## Entity: User

**Purpose**: Represents a system user authenticated via the existing identity provider.

**Fields**:

- `userId`: stable identity reference from the existing authentication system
- `displayReference`: pseudonymized reference usable in logs and admin views
- `activeRole`: current assigned role or null when not yet assigned
- `status`: active, inactive, or suspended according to the host application model

**Validation Rules**:

- A user can have at most one active business role in v1.
- Users without an active role must not receive protected business access.

## Entity: Role Assignment

**Purpose**: Tracks the current role of a user and the last administrative change that set it.

**Fields**:

- `assignmentId`: unique identifier of the role assignment record
- `userId`: reference to the assigned user
- `roleName`: assigned role value
- `assignedBy`: admin identity that performed the latest change
- `assignedAt`: timestamp of the latest change
- `changeReason`: optional administrative note for support or audit

**Validation Rules**:

- Each user has at most one active assignment record in v1.
- `assignedBy` is mandatory for all create, update, and revoke operations performed through the admin flow.

## Entity: Protected Area

**Purpose**: Represents a feature, route, screen, or API capability requiring authorization.

**Fields**:

- `areaKey`: stable business identifier for the protected area
- `description`: business-readable explanation of the area
- `allowedRoles`: roles permitted to access the area
- `customerScoped`: indicates whether the area must also enforce ownership or assignment boundaries

**Validation Rules**:

- Every protected area must declare its allowed roles explicitly.
- Customer-scoped areas must enforce both authentication and record-level access boundaries.

## Entity: Authorization Decision

**Purpose**: Captures the outcome of evaluating access to an area or action.

**Fields**:

- `userId`: evaluated user reference
- `roleName`: role used for the decision, or null when unassigned
- `target`: protected area or action key
- `result`: allowed or denied
- `reasonCode`: structured reason such as `missing_role`, `wrong_role`, `wrong_scope`, or `inactive_user`
- `evaluatedAt`: timestamp of the decision

**Validation Rules**:

- Denied decisions must resolve to a non-empty reason code.
- Decision records used for logging must exclude raw secrets and unnecessary PII.

## Entity: Audit Event

**Purpose**: Represents a structured business log entry for role changes and authorization-relevant events.

**Fields**:

- `eventType`: `role_assigned`, `role_changed`, `role_removed`, or `access_denied`
- `actorRef`: pseudonymized actor reference
- `subjectRef`: pseudonymized subject reference where applicable
- `target`: affected area, action, or role assignment
- `outcome`: success or denied
- `occurredAt`: event timestamp

**Validation Rules**:

- Every role assignment mutation creates an audit event.
- Every denied access on protected business paths creates an audit event.
