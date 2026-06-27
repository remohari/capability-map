# Authorization Contract: Rollenkonzept

## Purpose

Define the expected contract between admin role-management flows, protected backend capabilities, and frontend access behavior for the first RBAC release.

## Role Catalog

| Role | Primary Purpose | Must Be Allowed | Must Be Denied |
|------|------------------|-----------------|----------------|
| `Admin` | Rollen verwalten und Systemzugriffe steuern | Rollen zuweisen, Rollen aendern, geschuetzte Verwaltungsbereiche nutzen | n/a in this feature scope |
| `Kund:in` | Eigene Anliegen und zugehoerige Informationen nutzen | Eigene freigegebene Inhalte sehen und nutzen | Fremde Kundendaten, Verwaltungsbereiche, Rollenverwaltung |
| `Berater:in` | Beratungsfaelle im erlaubten Rahmen bearbeiten | Zugeordnete Beratungsinformationen sehen und bearbeiten | Rollenverwaltung, nicht freigegebene Bereiche |
| `Bewerter:in` | Bewertungsrelevante Inhalte einsehen | Bewertungsinformationen sehen | Rollenverwaltung, administrative Bereiche |

## Role Assignment Contract

- Only authenticated admins may create, change, or remove a business role assignment.
- Every successful role assignment mutation must persist exactly one active role for the target user.
- Every successful role assignment mutation must emit a structured audit event.
- Any attempted role assignment by a non-admin must be denied and logged as a denied event.

## Authorization Evaluation Contract

- Every protected backend route or business operation must evaluate:
  - authenticated identity
  - active role presence
  - role permission for the requested area or action
  - customer or assignment scope where applicable
- If any check fails, the result is `denied`.
- Denied results must produce:
  - a stable backend denial response
  - a user-facing frontend feedback state
  - a structured audit log entry

## Frontend Behavior Contract

- Frontend navigation should only expose role-allowed areas by default.
- Direct navigation to a protected route without permission must resolve to an access-denied state or redirect with equivalent feedback.
- Admin-only controls must be hidden or disabled for non-admin users, but backend enforcement remains mandatory.

## Logging Contract

- Required event types:
  - `role_assigned`
  - `role_changed`
  - `role_removed`
  - `access_denied`
- Each event must include:
  - timestamp
  - actor reference
  - subject reference when applicable
  - target area or target role
  - outcome
  - structured reason for denials
- Logs must not contain raw tokens or unnecessary personal data.
