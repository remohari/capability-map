# Quickstart: Startseite Kundenbereiche

## Goal

Deliver a homepage that shows authorized customer area tiles, includes mandatory navigation entries, handles empty/error states clearly, and supports search + pagination at scale.

## Recommended Delivery Order

1. Create backend tests first for:
   - returning only authorized customer areas
   - empty-state response when no areas are accessible
   - search filtering behavior
   - pagination metadata correctness
   - denied access handling and structured logging
2. Implement backend list endpoint/service updates until those tests pass.
3. Create frontend tests first for:
   - rendering area tiles when data exists
   - rendering empty-state message when result is empty
   - rendering user-facing error message on fetch failure
   - navigation presence (`home`, `customer-areas`)
   - search + pagination interaction states
4. Implement frontend homepage and navigation behavior until tests pass.
5. Run validation gates:
   - backend tests
   - frontend tests/type checks
   - lint/format checks
   - dependency audit check

## Acceptance Walkthrough

1. Sign in as a user with access to multiple areas and open the homepage.
2. Verify visible tiles match authorized customer areas only.
3. Use search to locate a known area and verify filtered result.
4. Navigate across pages and verify counts and boundaries.
5. Sign in as a user with no area access and verify empty-state text.
6. Trigger a backend failure scenario and verify clear frontend error feedback.
7. Review logs for `areas_loaded`, `areas_empty`, and denied/error outcomes.

## Implementation Notes

- Keep authorization enforcement server-side; frontend visibility is convenience only.
- Do not reintroduce GIF requirement in this feature increment.
- Reuse existing logo asset from `frontend/public/asserts/Nordmap.png`.
- Preserve minimum navigation scope from clarification: home + customer-areas.
- Keep API changes additive and backward compatible where possible.
- In this repository, backend code compiles and can be validated with `dotnet build`; frontend automated test runner is not initialized yet, so test scenarios are tracked in `frontend/tests/` until a runner is added.
