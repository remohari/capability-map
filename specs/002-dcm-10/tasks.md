# Tasks: Bereich/Kunde anlegen und kundenbezogene Zugriffsrechte

**Input**: Design documents from `/specs/002-dcm-10/`
**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Tests**: Tests are required for this feature by the constitution and plan. Backend area-creation and customer-access tests must be written first and verified as failing before implementation. Frontend access-state tests must be added before UI behavior changes.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Current Status

- Last updated: 2026-06-27
- Total tasks: 47
- Completed tasks: 22
- Open tasks: 25
- Progress: 46.8%

Maintain this section when task checkboxes change.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Path Conventions

- **Web app**: `backend/`, `frontend/`
- **Backend tests**: `backend/tests/Unit/`, `backend/tests/Integration/`
- **Frontend tests**: `frontend/tests/`

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Prepare the implementation repositories and shared area management scaffolding targets

- [x] T001 Confirm or create the feature branch `002-dcm-10` in the application repository and align it with `specs/002-dcm-10/`
- [x] T002 [P] Add area management API surface placeholders to `backend/src/Api/Controllers/AreaController.cs`
- [x] T003 [P] Add frontend area management route placeholders in `frontend/src/app/features/admin/area-routes.ts`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core area management infrastructure that MUST be complete before ANY user story can be implemented

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

- [ ] T004 Create failing backend unit tests for area creation validation rules in `backend/tests/Unit/Security/AreaCreationServiceTests.cs`
- [ ] T005 [P] Create failing backend integration tests for customer-scoped access filtering in `backend/tests/Integration/Security/CustomerAccessPolicyTests.cs`
- [ ] T006 [P] Create failing backend integration tests for forbidden area access responses in `backend/tests/Integration/Api/AreaAccessTests.cs`
- [ ] T007 [P] Create failing backend integration tests for structured audit logging of area creation and permission changes in `backend/tests/Integration/Security/AreaAuditLoggingTests.cs`
- [x] T008 Implement the `Kunde`, `Bereich`, `Bereichsberechtigung`, and area authorization decision models in `backend/src/Domain/Security/`
- [x] T009 Implement the area management application service and permission evaluation in `backend/src/Application/Security/Areas/`
- [x] T010 Implement structured logging helpers for area creation, permission changes, and access-denied events in `backend/src/Infrastructure/Logging/AreaEventLogger.cs`
- [x] T011 Implement frontend area state primitives and access-denied state model in `frontend/src/app/core/auth/area-state.ts`

**Checkpoint**: Foundation ready - user story implementation can now begin in parallel

---

## Phase 3: User Story 1 - Neuen Bereich/Kundenkontext anlegen (Priority: P1) 🎯 MVP

**Goal**: Allow authorized administrators to create new areas with associated customer contexts

**Independent Test**: An admin can create a new area with valid customer context, the area is persisted with correct customer association, and the area is visible in the area list.

### Tests for User Story 1 ⚠️

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [x] T012 [P] [US1] Create failing backend unit tests for area creation with valid customer context in `backend/tests/Unit/Security/AreaCreationServiceTests.cs`
- [ ] T013 [P] [US1] Create failing backend unit tests for area creation validation (missing required fields) in `backend/tests/Unit/Security/AreaCreationValidationTests.cs`
- [x] T014 [P] [US1] Create failing backend integration tests for area creation endpoints in `backend/tests/Integration/Api/AreaCreationControllerTests.cs`
- [ ] T015 [P] [US1] Create failing frontend tests for area creation form validation in `frontend/tests/area-creation/area-creation-form.spec.ts`

### Implementation for User Story 1

- [ ] T016 [P] [US1] Implement the `Kunde` and `Bereich` entity models and persistence mappings in `backend/src/Domain/Security/Kunde.cs`, `backend/src/Domain/Security/Bereich.cs`, and `backend/src/Infrastructure/Persistence/Configurations/KundeConfiguration.cs`, `backend/src/Infrastructure/Persistence/Configurations/BereichConfiguration.cs`
- [x] T017 [P] [US1] Implement request/response contracts for area creation operations in `backend/src/Application/Security/Areas/CreateAreaRequest.cs`, `backend/src/Application/Security/Areas/CreateAreaResponse.cs`
- [x] T018 [US1] Implement the area creation application service in `backend/src/Application/Security/Areas/AreaCreationService.cs`
- [x] T019 [US1] Implement admin-only area creation endpoints in `backend/src/Api/Controllers/AreaController.cs`
- [x] T020 [US1] Add structured area creation logging in `backend/src/Application/Security/Areas/AreaCreationService.cs`
- [x] T021 [US1] Implement the frontend area creation form and validation in `frontend/src/app/features/admin/area-creation/area-creation.component.ts`
- [x] T022 [US1] Connect frontend area creation actions to the backend API in `frontend/src/app/features/admin/area-creation/area-creation.service.ts`
- [x] T023 [US1] Add access-denied feedback for non-admin attempts to reach area-creation views in `frontend/src/app/core/auth/admin-access.guard.ts`

**Checkpoint**: At this point, User Story 1 should be fully functional and testable independently

---

## Phase 4: User Story 2 - Bereich mit eigenen Berechtigungen verwalten (Priority: P2)

**Goal**: Allow administrators to manage per-area access rights and ensure permissions are isolated per area

**Independent Test**: An admin can assign different permissions to two different areas, and the permissions remain isolated and do not leak across areas.

### Tests for User Story 2 ⚠️

- [ ] T024 [P] [US2] Create failing backend unit tests for per-area permission assignment and isolation in `backend/tests/Unit/Security/AreaPermissionServiceTests.cs`
- [ ] T025 [P] [US2] Create failing backend integration tests for area permission management endpoints in `backend/tests/Integration/Api/AreaPermissionControllerTests.cs`
- [ ] T026 [P] [US2] Create failing frontend tests for area permission management UI state in `frontend/tests/area-permissions/area-permission-management.spec.ts`

### Implementation for User Story 2

- [ ] T027 [P] [US2] Implement the `Bereichsberechtigung` entity model and persistence mapping in `backend/src/Domain/Security/Bereichsberechtigung.cs` and `backend/src/Infrastructure/Persistence/Configurations/BereichsberechtigungConfiguration.cs`
- [x] T028 [US2] Implement request/response contracts for area permission operations in `backend/src/Application/Security/Areas/GrantPermissionRequest.cs`, `backend/src/Application/Security/Areas/RevokePermissionRequest.cs`
- [x] T029 [US2] Implement the area permission application service in `backend/src/Application/Security/Areas/AreaPermissionService.cs`
- [x] T030 [US2] Implement area permission management endpoints in `backend/src/Api/Controllers/AreaPermissionController.cs`
- [x] T031 [US2] Add structured permission change logging in `backend/src/Application/Security/Areas/AreaPermissionService.cs`
- [x] T032 [US2] Implement the frontend area permission management form in `frontend/src/app/features/admin/area-permissions/area-permission-management.component.ts`
- [x] T033 [US2] Connect frontend area permission actions to the backend API in `frontend/src/app/features/admin/area-permissions/area-permission-management.service.ts`

**Checkpoint**: At this point, User Stories 1 AND 2 should both work independently

---

## Phase 5: User Story 3 - Kunden auf eigene Bereiche begrenzen (Priority: P3)

**Goal**: Ensure customers can only see and access areas that have been explicitly granted to them

**Independent Test**: A user with area permissions can only see their granted areas, while attempts to access other customers' areas are denied with clear feedback.

### Tests for User Story 3 ⚠️

- [ ] T034 [P] [US3] Create failing backend integration tests for customer-scoped area list filtering in `backend/tests/Integration/Security/CustomerAreaListFilteringTests.cs`
- [ ] T035 [P] [US3] Create failing backend integration tests for forbidden area access responses in `backend/tests/Integration/Api/CustomerAreaAccessTests.cs`
- [ ] T036 [P] [US3] Create failing frontend tests for customer area visibility and access-denied behavior in `frontend/tests/customer-access/customer-area-routes.spec.ts`

### Implementation for User Story 3

- [ ] T037 [P] [US3] Implement customer-scope authorization rules for area access in `backend/src/Security/CustomerAreaAccessPolicy.cs`
- [ ] T038 [US3] Apply customer-scope enforcement to area list and area detail endpoints in `backend/src/Api/Controllers/AreaController.cs`
- [ ] T039 [US3] Add denied-access logging and response mapping for customer-scope violations in `backend/src/Infrastructure/Logging/AreaEventLogger.cs` and `backend/src/Api/Middleware/CustomerScopeMiddleware.cs`
- [ ] T040 [US3] Implement customer role-aware area route configuration in `frontend/src/app/core/auth/area-routes.ts`
- [ ] T041 [US3] Implement customer access-denied page or redirect flow in `frontend/src/app/shared/access-denied/area-access-denied.component.ts`
- [ ] T042 [US3] Filter area list navigation to show only customer-granted areas in `frontend/src/app/core/navigation/area-navigation.component.ts`

**Checkpoint**: All user stories should now be independently functional

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories

- [ ] T043 [P] Update the final area management documentation in `specs/002-dcm-10/quickstart.md` and supporting product documentation
- [ ] T044 Verify structured logging coverage for `area_created`, `permission_granted`, `permission_revoked`, and `access_denied` across `backend/src/`
- [ ] T045 [P] Add any remaining shared frontend tests for access-denied rendering and area navigation regressions in `frontend/tests/access-denied/`
- [ ] T046 Run the full validation flow from `specs/002-dcm-10/quickstart.md` and capture any gaps in `specs/002-dcm-10/tasks.md`
- [ ] T047 Perform OWASP A01 and A09 review for all new area routes and logging paths in `backend/src/Api/`, `backend/src/Security/`, and `frontend/src/app/core/auth/`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories
- **User Stories (Phase 3+)**: All depend on Foundational phase completion
- **Polish (Phase 6)**: Depends on all desired user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational (Phase 2) - no dependency on other stories
- **User Story 2 (P2)**: Can start after Foundational (Phase 2) - reuses shared area management foundation but remains independently testable
- **User Story 3 (P3)**: Can start after Foundational (Phase 2) - reuses shared area management foundation but remains independently testable

### Within Each User Story

- Tests MUST be written and FAIL before implementation
- Domain and contract objects before services
- Services before controllers, guards, and UI integrations
- Logging and denial feedback must ship with the protected behavior, not as a later afterthought

### Parallel Opportunities

- `T002` and `T003` can run in parallel during setup
- `T005`, `T006`, and `T011` can run in parallel after `T004`
- Within US1, `T012`, `T013`, `T014`, and `T015` can run in parallel
- Within US2, `T024`, `T025`, and `T026` can run in parallel
- Within US3, `T034`, `T035`, and `T036` can run in parallel
- After Phase 2, different user stories can be staffed in parallel if coordination on shared auth files is managed carefully

---

## Parallel Example: User Story 1

```bash
# Launch all User Story 1 test creation tasks together:
Task: "Create failing backend unit tests for area creation with valid customer context in backend/tests/Unit/Security/AreaCreationServiceTests.cs"
Task: "Create failing backend unit tests for area creation validation (missing required fields) in backend/tests/Unit/Security/AreaCreationValidationTests.cs"
Task: "Create failing backend integration tests for area creation endpoints in backend/tests/Integration/Api/AreaCreationControllerTests.cs"
Task: "Create failing frontend tests for area creation form validation in frontend/tests/area-creation/area-creation-form.spec.ts"

# Launch independent User Story 1 model/contract tasks together:
Task: "Implement the Kunde and Bereich entity models and persistence mappings in backend/src/Domain/Security/Kunde.cs, backend/src/Domain/Security/Bereich.cs, and backend/src/Infrastructure/Persistence/Configurations/KundeConfiguration.cs, backend/src/Infrastructure/Persistence/Configurations/BereichConfiguration.cs"
Task: "Implement request/response contracts for area creation operations in backend/src/Application/Security/Areas/CreateAreaRequest.cs, backend/src/Application/Security/Areas/CreateAreaResponse.cs"
```

---

## Parallel Example: User Story 2

```bash
# Launch all User Story 2 test creation tasks together:
Task: "Create failing backend unit tests for per-area permission assignment and isolation in backend/tests/Unit/Security/AreaPermissionServiceTests.cs"
Task: "Create failing backend integration tests for area permission management endpoints in backend/tests/Integration/Api/AreaPermissionControllerTests.cs"
Task: "Create failing frontend tests for area permission management UI state in frontend/tests/area-permissions/area-permission-management.spec.ts"

# Launch independent User Story 2 model/contract tasks together:
Task: "Implement the Bereichsberechtigung entity model and persistence mapping in backend/src/Domain/Security/Bereichsberechtigung.cs and backend/src/Infrastructure/Persistence/Configurations/BereichsberechtigungConfiguration.cs"
Task: "Implement request/response contracts for area permission operations in backend/src/Application/Security/Areas/GrantPermissionRequest.cs, backend/src/Application/Security/Areas/RevokePermissionRequest.cs"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational
3. Complete Phase 3: User Story 1
4. **STOP and VALIDATE**: Confirm area creation with customer context works and non-admin attempts are blocked
5. Demo or merge the MVP slice if acceptable

### Incremental Delivery

1. Complete MVP (User Story 1)
2. Add User Story 2: Per-area permission management
3. Add User Story 3: Customer-scoped access enforcement
4. Complete Phase 6: Polish & Cross-Cutting Concerns
5. **FINAL VALIDATION**: Run full regression suite, OWASP review, and acceptance walkthrough
