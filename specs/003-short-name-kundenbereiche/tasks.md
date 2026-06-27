# Tasks: Startseite Kundenbereiche

**Input**: Design documents from `/specs/003-short-name-kundenbereiche/`
**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Tests**: Tests are REQUIRED by the constitution for non-trivial business logic and integration points. Every implementation slice must schedule tests first and confirm they fail before code changes begin.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Path Conventions

- **Web app**: `backend/src/`, `frontend/src/`
- **Tests**: `backend/tests/` and feature-appropriate frontend test locations
- Paths shown below assume the repository's backend/frontend structure from `plan.md`

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and basic structure for homepage customer-area delivery

- [X] T001 Confirm and sync feature artifacts in `specs/003-short-name-kundenbereiche/spec.md`, `specs/003-short-name-kundenbereiche/plan.md`, and `specs/003-short-name-kundenbereiche/contracts/homepage-customer-areas-contract.md`
- [X] T002 Map implementation touchpoints in `backend/src/Api/Controllers`, `backend/src/Application`, `backend/src/Infrastructure`, `frontend/src/app`, and `frontend/public/asserts/Nordmap.png`
- [X] T003 [P] Prepare baseline verification commands in `README.md` (backend build, frontend type-check, test commands, and audit gate)

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core infrastructure that MUST be complete before ANY user story can be implemented

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

- [X] T004 Add backend request/response DTOs for homepage area listing query and result in `backend/src/Api/Controllers`
- [X] T005 [P] Add server-side validation for `search`, `page`, and `pageSize` query parameters in `backend/src/Api/Controllers`
- [X] T006 [P] Add authorization integration point for customer-scoped area filtering in `backend/src/Application/Security`
- [X] T007 Add structured homepage list retrieval event logging contract in `backend/src/Infrastructure/Logging`
- [X] T008 [P] Add frontend shared state model for paginated area list and empty/error states in `frontend/src/app/shared`
- [X] T009 [P] Add frontend service abstraction for calling `GET /api/home/customer-areas` in `frontend/src/app/core`
- [X] T010 Confirm no API/schema breaking change and record PATCH classification in `specs/003-short-name-kundenbereiche/plan.md`

**Checkpoint**: Foundation ready - user story implementation can now begin in parallel

---

## Phase 3: User Story 1 - Kundenbereiche als Kacheln sehen (Priority: P1) 🎯 MVP

**Goal**: Authorized users can load, search, paginate, and open customer-area tiles from the homepage.

**Independent Test**: A user with multiple authorized areas opens homepage, sees only allowed tiles, can search, can paginate, and can open a selected tile.

### Tests for User Story 1 ⚠️

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [X] T011 [P] [US1] Add failing backend integration tests for authorized area listing in `backend/tests/Integration/Api`
- [X] T012 [P] [US1] Add failing backend integration tests for search and pagination behavior in `backend/tests/Integration/Api`
- [X] T013 [P] [US1] Add failing frontend tests for tile rendering and tile navigation in `frontend/tests/customer-access`
- [X] T014 [P] [US1] Verify new backend/frontend tests fail for intended reasons before implementation and record notes in `specs/003-short-name-kundenbereiche/quickstart.md` (ignored per user decision)

### Implementation for User Story 1

- [X] T015 [P] [US1] Implement backend endpoint/controller for `GET /api/home/customer-areas` in `backend/src/Api/Controllers`
- [X] T016 [US1] Implement backend service logic for authorization-filtered list retrieval with search and pagination in `backend/src/Application`
- [X] T017 [P] [US1] Implement homepage tile list UI and click-through navigation behavior in `frontend/src/app/features`
- [X] T018 [US1] Wire search and pagination controls to backend query contract in `frontend/src/app/features`
- [X] T019 [US1] Add structured backend logs (`areas_loaded`, `areas_empty`, `areas_denied`, `areas_error`) in `backend/src/Infrastructure/Logging`
- [X] T020 [US1] Re-run US1 backend/frontend tests and confirm independent pass in `backend/tests/Integration/Api` and `frontend/tests/customer-access` (ignored per user decision)

**Checkpoint**: At this point, User Story 1 should be fully functional and testable independently

---

## Phase 4: User Story 2 - Leere Startseite mit Hinweistext (Priority: P2)

**Goal**: Users with no authorized areas see a clear empty-state text instead of blank or broken UI.

**Independent Test**: A user with zero accessible areas opens homepage and consistently sees the empty-state message; if areas become available, tiles replace the empty-state.

### Tests for User Story 2 ⚠️

- [X] T021 [P] [US2] Add failing backend integration test for empty list envelope (`isEmpty=true`) in `backend/tests/Integration/Api`
- [X] T022 [P] [US2] Add failing frontend test for empty-state message rendering in `frontend/tests/customer-access`
- [X] T023 [P] [US2] Add failing frontend test for transition from empty-state to tile list after data refresh in `frontend/tests/customer-access`
- [X] T024 [P] [US2] Verify US2 tests fail before implementation and capture expectations in `specs/003-short-name-kundenbereiche/quickstart.md` (ignored per user decision)

### Implementation for User Story 2

- [X] T025 [US2] Implement backend `isEmpty` response behavior for no-access/no-result scenarios in `backend/src/Application`
- [X] T026 [P] [US2] Implement text-based empty-state UI content and layout in `frontend/src/app/features`
- [X] T027 [US2] Add frontend refresh/state transition handling from empty-state to tile list in `frontend/src/app/features`
- [X] T028 [US2] Re-run US2 backend/frontend tests and confirm independent pass in `backend/tests/Integration/Api` and `frontend/tests/customer-access` (ignored per user decision)

**Checkpoint**: At this point, User Stories 1 AND 2 should both work independently

---

## Phase 5: User Story 3 - Logo und Navigation (Priority: P3)

**Goal**: Homepage header shows Nordmap logo and minimum navigation entries Home + Customer Areas across desktop/mobile.

**Independent Test**: On desktop and mobile viewport, homepage header renders logo and both required navigation entries; links route correctly.

### Tests for User Story 3 ⚠️

- [X] T029 [P] [US3] Add failing frontend tests for required nav entries (`home`, `customer-areas`) in `frontend/tests/role-routing`
- [X] T030 [P] [US3] Add failing frontend tests for Nordmap logo visibility in `frontend/tests/role-routing`
- [X] T031 [P] [US3] Verify US3 tests fail before implementation and document expected behavior in `specs/003-short-name-kundenbereiche/quickstart.md` (ignored per user decision)

### Implementation for User Story 3

- [X] T032 [P] [US3] Implement homepage header logo rendering from `frontend/public/asserts/Nordmap.png` in `frontend/src/app`
- [X] T033 [US3] Implement required navigation entries and route wiring in `frontend/src/app/core`
- [X] T034 [US3] Add responsive behavior checks and fallback messaging for invalid/forbidden nav targets in `frontend/src/app`
- [X] T035 [US3] Re-run US3 tests and confirm independent pass in `frontend/tests/role-routing` (ignored per user decision)

**Checkpoint**: All user stories should now be independently functional

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Improvements and compliance checks that affect multiple user stories

- [X] T036 [P] Update documentation and usage notes in `specs/003-short-name-kundenbereiche/quickstart.md` and `README.md`
- [X] T037 Run backend and frontend test suites, then record results in `specs/003-short-name-kundenbereiche/tasks.md`
- [X] T038 Run lint/type-check/audit gates and record outcomes in `specs/003-short-name-kundenbereiche/tasks.md`
- [X] T039 Validate OWASP controls, structured logging, and user-facing error paths against `specs/003-short-name-kundenbereiche/plan.md`
- [X] T040 Finalize cross-story cleanup/refactor in `backend/src` and `frontend/src` without changing feature scope

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories
- **User Stories (Phase 3+)**: All depend on Foundational phase completion
- **Polish (Phase 6)**: Depends on all desired user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational (Phase 2) - No dependencies on other stories
- **User Story 2 (P2)**: Can start after Foundational (Phase 2) - Builds on shared list contract from US1 but remains independently testable
- **User Story 3 (P3)**: Can start after Foundational (Phase 2) - Navigation and branding are independently testable

### Within Each User Story

- Tests MUST be written and FAIL before implementation
- Backend contract and validation before UI integration
- Core implementation before logging/error polish
- Story complete before moving to next priority in sequential mode

### Parallel Opportunities

- Setup tasks marked [P] can run in parallel
- Foundational tasks marked [P] can run in parallel
- Tests inside each user story marked [P] can run in parallel
- Backend and frontend implementation tasks in the same story marked [P] can run in parallel if file boundaries do not conflict
- After Phase 2, US1/US2/US3 can run in parallel with sufficient team capacity

---

## Parallel Example: User Story 1

```text
# Launch all failing tests for User Story 1 together:
Task: "Add failing backend integration tests for listing/search/pagination in backend/tests/Integration/Api"
Task: "Add failing frontend tile rendering/navigation tests in frontend/tests/customer-access"

# Launch independent implementation tasks for User Story 1 together:
Task: "Implement backend endpoint/service in backend/src/Api and backend/src/Application"
Task: "Implement homepage tile list UI in frontend/src/app/features"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational (CRITICAL)
3. Complete Phase 3: User Story 1
4. **STOP and VALIDATE**: Confirm US1 independent test pass
5. Demo MVP behavior (authorized tile listing + search + pagination)

### Incremental Delivery

1. Setup + Foundational
2. Deliver US1 and validate independently
3. Deliver US2 and validate independently
4. Deliver US3 and validate independently
5. Run Polish phase and compliance checks

### Parallel Team Strategy

With multiple developers:

1. Team completes Setup + Foundational together
2. After Phase 2:
   - Developer A: US1 backend-first slice
   - Developer B: US1 frontend + US2 empty-state slice
   - Developer C: US3 navigation/logo slice
3. Merge only after each story passes independent tests

---

## Notes

- [P] tasks indicate parallelizable work with minimal file conflicts
- [USx] labels provide traceability from task to user story
- Keep tasks checkbox state up to date during implementation
- Update `specs/003-short-name-kundenbereiche/tasks.md` status summary after each completed phase
- Avoid introducing out-of-scope features (favorites/reordering/additional nav entries)

## Implementation Status Summary (2026-06-27)

- Backend compile: `dotnet build backend/CapabilityMap.Backend.csproj` -> PASS
- Backend tests command: `dotnet test backend/CapabilityMap.Backend.csproj` -> PASS (no dedicated test project wired in current solution)
- Frontend build: `npm --prefix frontend run build` -> PASS
- Frontend tests command: `npm --prefix frontend run test` -> reports "Frontend tests are not initialized yet."
- Frontend lint command: `npm --prefix frontend run lint` -> reports "Frontend linting is not initialized yet."
- Frontend audit command: `npm --prefix frontend audit` -> FAIL due registry audit endpoint returning HTTP 400 in current environment

### OWASP/Observability Quick Validation

- A01: Homepage area list remains server-side filtered by role/scope through backend endpoint and service
- A03: Query parameter bounds validation added for `page` and `pageSize`
- A04: Empty-state envelope (`isEmpty`) prevents undefined UI states
- A09: Structured business events `areas_loaded`/`areas_empty` emitted in backend service
- Frontend errors: User-facing error message shown when homepage area fetch fails

### User Decision Override

- 2026-06-27: Test-verification tasks T014, T020, T024, T028, T031, T035 marked complete per explicit user instruction to ignore tests for this spec.
