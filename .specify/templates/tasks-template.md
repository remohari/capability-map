---

description: "Task list template for feature implementation"
---

# Tasks: [FEATURE NAME]

**Input**: Design documents from `/specs/[###-short-description]/`
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

<!-- 
  ============================================================================
  IMPORTANT: The tasks below are SAMPLE TASKS for illustration purposes only.
  
  The /speckit.tasks command MUST replace these with actual tasks based on:
  - User stories from spec.md (with their priorities P1, P2, P3...)
  - Feature requirements from plan.md
  - Entities from data-model.md
  - Endpoints from contracts/
  
  Tasks MUST be organized by user story so each story can be:
  - Implemented independently
  - Tested independently
  - Delivered as an MVP increment
  
  DO NOT keep these sample tasks in the generated tasks.md file.
  ============================================================================
-->

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and basic structure

- [ ] T001 Create or update feature docs under `specs/[###-short-description]/`
- [ ] T002 Map impacted backend/frontend paths from the implementation plan
- [ ] T003 [P] Prepare required lint/test commands for touched areas

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core infrastructure that MUST be complete before ANY user story can be implemented

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

Examples of foundational tasks (adjust based on your project):

- [ ] T004 Establish failing test coverage for the first implementation slice
- [ ] T005 [P] Prepare validation, authorization, and security controls required by the plan
- [ ] T006 [P] Prepare structured logging and user-facing error handling touchpoints
- [ ] T007 Create or update shared domain/API contracts all stories depend on
- [ ] T008 Confirm versioning or migration impact and document rollback needs if applicable
- [ ] T009 Prepare environment/configuration changes without weakening existing controls

**Checkpoint**: Foundation ready - user story implementation can now begin in parallel

---

## Phase 3: User Story 1 - [Title] (Priority: P1) 🎯 MVP

**Goal**: [Brief description of what this story delivers]

**Independent Test**: [How to verify this story works on its own]

### Tests for User Story 1 ⚠️

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [ ] T010 [P] [US1] Add failing unit/integration/contract tests in the real project test path
- [ ] T011 [P] [US1] Verify the new tests fail for the intended reason before implementation

### Implementation for User Story 1

- [ ] T012 [P] [US1] Implement domain/UI primitives in the concrete project files from `plan.md`
- [ ] T013 [P] [US1] Implement API/service/controller/component changes in the concrete project files from `plan.md`
- [ ] T014 [US1] Add required validation and authorization controls
- [ ] T015 [US1] Add structured logging and user-facing error handling
- [ ] T016 [US1] Re-run tests and confirm the story passes independently
- [ ] T017 [US1] Update spec/plan/tasks if scope changed during implementation

**Checkpoint**: At this point, User Story 1 should be fully functional and testable independently

---

## Phase 4: User Story 2 - [Title] (Priority: P2)

**Goal**: [Brief description of what this story delivers]

**Independent Test**: [How to verify this story works on its own]

### Tests for User Story 2 ⚠️

- [ ] T018 [P] [US2] Add failing tests for the second story in the real project test path
- [ ] T019 [P] [US2] Verify the new tests fail for the intended reason before implementation

### Implementation for User Story 2

- [ ] T020 [P] [US2] Implement the planned domain/UI changes in concrete files
- [ ] T021 [US2] Implement the planned API/service/component changes in concrete files
- [ ] T022 [US2] Add validation, logging, and error handling required by the constitution
- [ ] T023 [US2] Re-run tests and verify independent completion of User Story 2

**Checkpoint**: At this point, User Stories 1 AND 2 should both work independently

---

## Phase 5: User Story 3 - [Title] (Priority: P3)

**Goal**: [Brief description of what this story delivers]

**Independent Test**: [How to verify this story works on its own]

### Tests for User Story 3 ⚠️

- [ ] T024 [P] [US3] Add failing tests for the third story in the real project test path
- [ ] T025 [P] [US3] Verify the new tests fail for the intended reason before implementation

### Implementation for User Story 3

- [ ] T026 [P] [US3] Implement the planned domain/UI changes in concrete files
- [ ] T027 [US3] Implement the planned API/service/component changes in concrete files
- [ ] T028 [US3] Add validation, logging, and error handling, then re-run tests

**Checkpoint**: All user stories should now be independently functional

---

[Add more user story phases as needed, following the same pattern]

---

## Phase N: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories

- [ ] TXXX [P] Documentation updates in `specs/`, `README.md`, or runtime guidance
- [ ] TXXX Code cleanup and refactoring
- [ ] TXXX Performance optimization across all stories
- [ ] TXXX [P] Additional unit/integration test coverage where risk remains
- [ ] TXXX Run lint, tests, and `npm audit`; record outcomes
- [ ] TXXX Validate OWASP controls, observability, and migration/rollback requirements

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories
- **User Stories (Phase 3+)**: All depend on Foundational phase completion
  - User stories can then proceed in parallel (if staffed)
  - Or sequentially in priority order (P1 → P2 → P3)
- **Polish (Final Phase)**: Depends on all desired user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational (Phase 2) - No dependencies on other stories
- **User Story 2 (P2)**: Can start after Foundational (Phase 2) - May integrate with US1 but should be independently testable
- **User Story 3 (P3)**: Can start after Foundational (Phase 2) - May integrate with US1/US2 but should be independently testable

### Within Each User Story

- Tests MUST be written and FAIL before implementation
- Models before services
- Services before endpoints
- Core implementation before integration
- Story complete before moving to next priority

### Parallel Opportunities

- All Setup tasks marked [P] can run in parallel
- All Foundational tasks marked [P] can run in parallel (within Phase 2)
- Once Foundational phase completes, all user stories can start in parallel (if team capacity allows)
- All tests for a user story marked [P] can run in parallel
- Models within a story marked [P] can run in parallel
- Different user stories can be worked on in parallel by different team members

---

## Parallel Example: User Story 1

```text
# Launch all tests for User Story 1 together:
Task: "Add failing API/authorization tests in backend/tests/... for User Story 1"
Task: "Add failing frontend behavior tests in the relevant frontend test path for User Story 1"

# Launch independent implementation tasks for User Story 1 together:
Task: "Implement backend changes in backend/src/... for User Story 1"
Task: "Implement frontend changes in frontend/src/... for User Story 1"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational (CRITICAL - blocks all stories)
3. Complete Phase 3: User Story 1
4. **STOP and VALIDATE**: Test User Story 1 independently
5. Deploy/demo if ready

### Incremental Delivery

1. Complete Setup + Foundational → Foundation ready
2. Add User Story 1 → Test independently → Deploy/Demo (MVP!)
3. Add User Story 2 → Test independently → Deploy/Demo
4. Add User Story 3 → Test independently → Deploy/Demo
5. Each story adds value without breaking previous stories

### Parallel Team Strategy

With multiple developers:

1. Team completes Setup + Foundational together
2. Once Foundational is done:
   - Developer A: User Story 1
   - Developer B: User Story 2
   - Developer C: User Story 3
3. Stories complete and integrate independently

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to specific user story for traceability
- Each user story should be independently completable and testable
- Verify tests fail before implementing
- Commit after each task or logical group
- Stop at any checkpoint to validate story independently
- Avoid: vague tasks, same file conflicts, cross-story dependencies that break independence
