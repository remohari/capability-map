# Quickstart: Bereich/Kunde anlegen und kundenbezogene Zugriffsrechte

## Goal

Deliver a customer area management system with test-first enforcement of area creation, per-area permissions, and customer-scoped access boundaries.

## Recommended Delivery Order

1. Create backend tests for:
   - creating a new area with valid customer context
   - rejecting area creation with incomplete required fields
   - preventing duplicate areas for the same customer
   - enforcing customer-scoped access filtering
   - blocking access to areas without explicit permissions
   - denying access to other customers' areas
2. Implement the backend area model, area-creation workflow, permission management, customer-scope enforcement, and structured logging until those tests pass.
3. Create frontend tests for:
   - area creation form validation
   - area list visibility filtered by customer scope
   - access-denied feedback when a restricted area is opened without permission
   - permission management UI for administrators
4. Implement frontend area creation forms, permission matrices, customer-scope filtering, and denial messaging until those tests pass.
5. Run full regression checks in the application repository:
   - backend unit and integration tests
   - frontend automated tests
   - lint and formatting gates
   - dependency audit gate

## Acceptance Walkthrough

1. Sign in as an admin and create a new area with customer context.
2. Confirm the area is created with a valid customer association and is visible in the area list.
3. Grant area permissions to a user for the newly created area.
4. Sign in as the granted user and confirm only the permitted area is visible.
5. Attempt to access another customer's area and confirm access is blocked with clear feedback.
6. Review logs for one successful area creation, one permission grant, and one denied access event.

## Implementation Notes

- Keep the v1 model single-role only with area-specific permissions.
- Centralize area permission checks in the backend authorization service.
- Treat frontend visibility as convenience and backend authorization as the real control.
- Areas without permissions must deny all customer access by default.
- Use the header-based demo identity contract in this repository for local validation:
  - `X-User-Id`
  - `X-Display-Reference`
  - `X-Active-Role`
  - `X-User-Status`
  - `X-Customer-Scope`
- Local verification in this repository should prefer:
  - `dotnet build backend/CapabilityMap.Backend.csproj`
  - `npx tsc -p frontend/tsconfig.app.json --noEmit`
  - manual browser validation of area creation, permission management, and access-denied flows
