# Tasks: Rollenkonzept

**Input**: Design documents from `/specs/001-rollenkonzept/`
**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Tests**: Tests are required for this feature by the constitution and plan. Backend authorization and role-assignment tests must be written first and verified as failing before implementation. Frontend access-state tests must be added before UI behavior changes.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Path Conventions

- **Web app**: `backend/`, `frontend/`
- **Backend tests**: `backend/tests/Unit/`, `backend/tests/Integration/`
- **Frontend tests**: `frontend/tests/`

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Prepare the implementation repositories and shared authorization scaffolding targets

- [x] T001 Confirm or create the feature branch `001-rollenkonzept` in the application repository and align it with `specs/001-rollenkonzept/`
- [x] T002 Document the initial four-role rights matrix in `backend/src/Security/RoleDefinitions.cs`
- [x] T003 [P] Add role-related API surface placeholders to `backend/src/Api/Controllers/RoleAssignmentsController.cs`
- [x] T004 [P] Add frontend route and navigation placeholders for protected areas in `frontend/src/app/core/auth/role-routes.ts`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core authorization infrastructure that MUST be complete before ANY user story can be implemented

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

- [x] T005 Create failing backend unit tests for role evaluation rules in `backend/tests/Unit/Security/RoleAuthorizationServiceTests.cs`
- [x] T006 [P] Create failing backend integration tests for protected-route denial and missing-role behavior in `backend/tests/Integration/Security/AuthorizationPolicyTests.cs`
- [x] T007 [P] Create failing backend integration tests for structured audit logging of denied access in `backend/tests/Integration/Security/AuthorizationAuditLoggingTests.cs`
- [x] T008 Implement the `Role`, `RoleAssignment`, and authorization decision models in `backend/src/Domain/Security/`
- [x] T009 Implement the shared authorization service and policy registration in `backend/src/Security/RoleAuthorizationService.cs` and `backend/src/Security/AuthorizationExtensions.cs`
- [x] T010 Implement structured logging helpers for role changes and access-denied events in `backend/src/Infrastructure/Logging/AuthorizationEventLogger.cs`
- [x] T011 Implement frontend role state primitives and access-denied state model in `frontend/src/app/core/auth/role-state.ts`

**Checkpoint**: Foundation ready - user story implementation can now begin in parallel

---

## Phase 3: User Story 1 - Rollen verwalten (Priority: P1) 🎯 MVP

**Goal**: Allow admins to assign, change, and remove exactly one active business role per user

**Independent Test**: An admin can assign or change a user role through the role-management flow, the current role is persisted and visible, and a non-admin attempting the same action is denied.

### Tests for User Story 1 ⚠️

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [x] T012 [P] [US1] Create failing backend unit tests for single active role assignment rules in `backend/tests/Unit/Security/RoleAssignmentServiceTests.cs`
- [x] T013 [P] [US1] Create failing backend integration tests for admin role assignment endpoints in `backend/tests/Integration/Api/RoleAssignmentsControllerTests.cs`
- [x] T014 [P] [US1] Create failing frontend tests for admin role-management UI state in `frontend/tests/role-management/role-management.component.spec.ts`

### Implementation for User Story 1

- [x] T015 [P] [US1] Implement the role assignment entity and persistence mapping in `backend/src/Domain/Security/RoleAssignment.cs` and `backend/src/Infrastructure/Persistence/Configurations/RoleAssignmentConfiguration.cs`
- [x] T016 [P] [US1] Implement request/response contracts for role assignment operations in `backend/src/Application/Security/RoleAssignments/`
- [x] T017 [US1] Implement the role assignment application service in `backend/src/Application/Security/RoleAssignments/RoleAssignmentService.cs`
- [x] T018 [US1] Implement admin-only role assignment endpoints in `backend/src/Api/Controllers/RoleAssignmentsController.cs`
- [x] T019 [US1] Add structured role assignment, role change, and role removal logging in `backend/src/Application/Security/RoleAssignments/RoleAssignmentService.cs`
- [x] T020 [US1] Implement the frontend admin role-management page and form in `frontend/src/app/features/admin/role-management/`
- [x] T021 [US1] Connect frontend role-management actions to the backend API in `frontend/src/app/features/admin/role-management/role-management.service.ts`
- [x] T022 [US1] Add access-denied feedback for non-admin attempts to reach role-management views in `frontend/src/app/core/auth/admin-access.guard.ts`

**Checkpoint**: At this point, User Story 1 should be fully functional and testable independently

---

## Phase 4: User Story 2 - Eigene Inhalte sicher nutzen (Priority: P2)

**Goal**: Ensure customers can only see and use their own allowed content and are blocked from other customers' data and admin areas

**Independent Test**: A user with role `Kund:in` can open only their own customer-scoped content, while attempts to access another customer’s data or admin-only areas are denied with clear feedback.

### Tests for User Story 2 ⚠️

- [x] T023 [P] [US2] Create failing backend integration tests for customer-scoped access filtering in `backend/tests/Integration/Security/CustomerAccessPolicyTests.cs`
- [x] T024 [P] [US2] Create failing backend integration tests for forbidden customer access responses in `backend/tests/Integration/Api/CustomerProtectedEndpointsTests.cs`
- [x] T025 [P] [US2] Create failing frontend tests for customer route visibility and access-denied behavior in `frontend/tests/customer-access/customer-protected-routes.spec.ts`

### Implementation for User Story 2

- [x] T026 [P] [US2] Implement customer-scope authorization rules in `backend/src/Security/CustomerAccessPolicy.cs`
- [x] T027 [US2] Apply customer-scope enforcement to protected read endpoints in `backend/src/Api/Controllers/Customer/`
- [x] T028 [US2] Add denied-access logging and response mapping for customer-scope violations in `backend/src/Infrastructure/Logging/AuthorizationEventLogger.cs` and `backend/src/Api/Middleware/`
- [x] T029 [US2] Implement customer role-aware route configuration in `frontend/src/app/core/auth/role-routes.ts`
- [x] T030 [US2] Implement customer access-denied page or redirect flow in `frontend/src/app/shared/access-denied/`
- [x] T031 [US2] Hide or disable non-customer-allowed navigation items for `Kund:in` in `frontend/src/app/core/navigation/`

**Checkpoint**: At this point, User Stories 1 AND 2 should both work independently

---

## Phase 5: User Story 3 - Beratungs- und Bewertungszugriff abgrenzen (Priority: P3)

**Goal**: Separate advisor and evaluator access so each role gets only the information and actions required for its task without administrative rights

**Independent Test**: A `Berater:in` can use advisor-allowed areas, a `Bewerter:in` can use evaluator-allowed areas, and both are blocked from admin functions with clear feedback.

### Tests for User Story 3 ⚠️

- [x] T032 [P] [US3] Create failing backend integration tests for advisor and evaluator role policies in `backend/tests/Integration/Security/AdvisorEvaluatorPolicyTests.cs`
- [x] T033 [P] [US3] Create failing backend integration tests for admin-area denial to advisor and evaluator roles in `backend/tests/Integration/Api/AdminAreaAuthorizationTests.cs`
- [x] T034 [P] [US3] Create failing frontend tests for advisor and evaluator protected navigation in `frontend/tests/role-routing/advisor-evaluator-routing.spec.ts`

### Implementation for User Story 3

- [x] T035 [P] [US3] Implement advisor and evaluator role capability mappings in `backend/src/Security/RoleDefinitions.cs`
- [x] T036 [US3] Apply advisor and evaluator authorization policies to protected backend areas in `backend/src/Api/Controllers/Advisor/` and `backend/src/Api/Controllers/Evaluator/`
- [x] T037 [US3] Implement advisor and evaluator route guards in `frontend/src/app/core/auth/role-access.guard.ts`
- [x] T038 [US3] Update advisor and evaluator feature modules to honor role-based access in `frontend/src/app/features/advisor/` and `frontend/src/app/features/evaluator/`
- [x] T039 [US3] Add explicit denial feedback for forbidden admin operations reached from advisor or evaluator flows in `frontend/src/app/shared/access-denied/`

**Checkpoint**: All user stories should now be independently functional

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories

- [x] T040 [P] Update the final rights matrix and implementation notes in `specs/001-rollenkonzept/quickstart.md` and supporting product documentation
- [ ] T041 Verify structured logging coverage for `role_assigned`, `role_changed`, `role_removed`, and `access_denied` across `backend/src/`
- [x] T042 [P] Add any remaining shared frontend tests for access-denied rendering and navigation regressions in `frontend/tests/access-denied/`
- [ ] T043 Run the full validation flow from `specs/001-rollenkonzept/quickstart.md` and capture any gaps in `specs/001-rollenkonzept/tasks.md`
- [ ] T044 Perform OWASP A01 and A09 review for all new protected routes and logging paths in `backend/src/Api/`, `backend/src/Security/`, and `frontend/src/app/core/auth/`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories
- **User Stories (Phase 3+)**: All depend on Foundational phase completion
- **Polish (Phase 6)**: Depends on all desired user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational (Phase 2) - no dependency on other stories
- **User Story 2 (P2)**: Can start after Foundational (Phase 2) - reuses shared authorization foundation but remains independently testable
- **User Story 3 (P3)**: Can start after Foundational (Phase 2) - reuses shared authorization foundation but remains independently testable

### Within Each User Story

- Tests MUST be written and FAIL before implementation
- Domain and contract objects before services
- Services before controllers, guards, and UI integrations
- Logging and denial feedback must ship with the protected behavior, not as a later afterthought

### Parallel Opportunities

- `T003` and `T004` can run in parallel during setup
- `T006`, `T007`, and `T011` can run in parallel after `T005`
- Within US1, `T012`, `T013`, and `T014` can run in parallel
- Within US2, `T023`, `T024`, and `T025` can run in parallel
- Within US3, `T032`, `T033`, and `T034` can run in parallel
- After Phase 2, different user stories can be staffed in parallel if coordination on shared auth files is managed carefully

---

## Parallel Example: User Story 1

```bash
# Launch all User Story 1 test creation tasks together:
Task: "Create failing backend unit tests for single active role assignment rules in backend/tests/Unit/Security/RoleAssignmentServiceTests.cs"
Task: "Create failing backend integration tests for admin role assignment endpoints in backend/tests/Integration/Api/RoleAssignmentsControllerTests.cs"
Task: "Create failing frontend tests for admin role-management UI state in frontend/tests/role-management/role-management.component.spec.ts"

# Launch independent User Story 1 model/contract tasks together:
Task: "Implement the role assignment entity and persistence mapping in backend/src/Domain/Security/RoleAssignment.cs and backend/src/Infrastructure/Persistence/Configurations/RoleAssignmentConfiguration.cs"
Task: "Implement request/response contracts for role assignment operations in backend/src/Application/Security/RoleAssignments/"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational
3. Complete Phase 3: User Story 1
4. **STOP and VALIDATE**: Confirm admin role assignment works and non-admin attempts are blocked
5. Demo or merge the MVP slice if acceptable

### Incremental Delivery

1. Complete Setup + Foundational
2. Add User Story 1 and validate independently
3. Add User Story 2 and validate customer-only visibility independently
4. Add User Story 3 and validate advisor/evaluator separation independently
5. Finish with Polish and explicit OWASP/logging review

### Parallel Team Strategy

1. Team completes Setup + Foundational together
2. Once Foundational is done:
   - Developer A: User Story 1
   - Developer B: User Story 2
   - Developer C: User Story 3
3. Coordinate changes to shared authorization files before merge

---

## Notes

- [P] tasks target different files or isolated scopes and are suitable for parallel execution
- [US1], [US2], and [US3] map directly to the approved user stories in `spec.md`
- Each user story remains independently testable after the shared foundation is in place
- The implementation repository, not this planning repository, is the place where the listed source paths are expected to exist
