# Research: Startseite Kundenbereiche

## Decision 1: Customer areas are fetched server-side with authorization filtering

**Decision**: The homepage must load customer area tiles from an authenticated backend endpoint that enforces customer-scope and role checks before returning any data.

**Rationale**: This guarantees A01 compliance and prevents exposure of unauthorized area metadata through client-only filtering.

**Alternatives considered**:

- Frontend-only filtering after loading all areas: rejected because it can leak restricted data.
- Static client-side mock list for first release: rejected because it does not satisfy real authorization behavior.

## Decision 2: Search and pagination are both mandatory in the list contract

**Decision**: Area listing supports text search and explicit pagination parameters from the first implementation increment.

**Rationale**: Clarification session explicitly selected search and pagination; both are needed to keep navigation usable with high area counts.

**Alternatives considered**:

- Search only with infinite list: rejected due to poor performance and scanability at higher data volume.
- Pagination only without search: rejected because users cannot quickly target a known area name.

## Decision 3: Empty state uses text-only guidance for now

**Decision**: When no areas are available, homepage shows a clear empty-state text and optional action hint, without requiring GIF/media assets.

**Rationale**: Clarification resolved that dancing-elk GIF is omitted for now; text-only empty state is simpler and reliable.

**Alternatives considered**:

- Mandatory GIF empty state: rejected by clarification outcome.
- Generic blank page: rejected because it fails user guidance expectations.

## Decision 4: Navigation minimum scope is fixed to Home and Customer Areas

**Decision**: Header navigation must include exactly the minimum required entries: Home and Customer Areas. Additional entries are deferred.

**Rationale**: Clarification defined this minimal scope and it aligns with YAGNI for the current feature.

**Alternatives considered**:

- Include role management in same feature: rejected as out of scope.
- Single-link navigation only: rejected because it does not meet clarified requirement.

## Decision 5: Structured observability for homepage retrieval outcomes

**Decision**: Backend logs must include structured events for area list retrieval (`success`, `empty`, `denied`, `error`) including request correlation and pseudonymized actor reference.

**Rationale**: Supports constitution observability gate and A09 monitoring requirements while keeping logs privacy-safe.

**Alternatives considered**:

- Log only exceptions: rejected because normal empty/denied outcomes are operationally relevant.
- Log full user identifiers or raw tokens: rejected due to privacy and security risk.

## Decision 6: API impact classification remains PATCH

**Decision**: Reuse existing protected listing shape where possible; additive query parameters for search/pagination are backward compatible and classified as PATCH.

**Rationale**: The feature adds behavior without breaking existing consumers.

**Alternatives considered**:

- New mandatory response format: rejected because it risks unnecessary client breakage.
- Separate endpoint for each filter mode: rejected as unnecessary complexity.
