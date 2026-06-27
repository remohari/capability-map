# Quickstart: Rollenkonzept

## Goal

Deliver the four-role authorization foundation with test-first enforcement across backend and frontend behavior.

## Recommended Delivery Order

1. Create backend authorization tests for:
   - assigning a role as admin
   - rejecting role changes from non-admin users
   - denying protected access when no active role exists
   - enforcing customer-only visibility for `Kund:in`
   - blocking administrative actions for `Berater:in` and `Bewerter:in`
2. Implement the backend role model, role-assignment workflow, policy checks, and structured logging until those tests pass.
3. Create frontend tests for:
   - route visibility by role
   - access-denied feedback when a protected view is opened without permission
   - hiding or disabling admin-only actions for non-admin users
4. Implement frontend role-aware navigation, protected route handling, and denial messaging until those tests pass.
5. Run full regression checks in the application repository:
   - backend unit and integration tests
   - frontend automated tests
   - lint and formatting gates
   - dependency audit gate

## Acceptance Walkthrough

1. Sign in as an admin and assign `Berater:in` to a user.
2. Confirm the assigned user can reach advisor-only areas but not admin-only functions.
3. Sign in as a customer and confirm only own records and customer-allowed functions are visible.
4. Attempt a forbidden route or action with each non-admin role and confirm access is blocked with clear feedback.
5. Review logs for one successful role change and one denied access event.

## Implementation Notes

- Keep the v1 model single-role only.
- Centralize the role matrix instead of scattering checks.
- Treat frontend visibility as convenience and backend authorization as the real control.
- Use the header-based demo identity contract in this repository for local validation:
  - `X-User-Id`
  - `X-Display-Reference`
  - `X-Active-Role`
  - `X-User-Status`
  - `X-Customer-Scope`
- Local verification in this repository should prefer:
  - `dotnet build backend/CapabilityMap.Backend.csproj`
  - `npx tsc -p frontend/tsconfig.app.json --noEmit`
  - manual browser validation of admin, customer, advisor, evaluator, and access-denied flows
