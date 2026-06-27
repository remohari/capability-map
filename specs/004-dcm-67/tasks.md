# Tasks: DCM-67 Bereichs- und Kundenzugriffsverwaltung

**Branch**: `004-dcm-67` | **Date**: 2026-06-27 | **Spec**: [spec.md](./spec.md)

## Status

- Total tasks: 26
- Completed: 0
- Open: 26
- Progress: 0%

> Aktualisiere dieses Statusfeld während der Umsetzung.

## Task Format

- `[ID] [P?] [Story] Beschreibung`
- **[P]**: Kann parallel bearbeitet werden
- **[Story]**: Zuordnung zur User Story (US1, US2, US3)
- Pfade zu konkreten Dateien einschließen

---

## Phase 1: Setup

- [ ] T001 [P] Kläre die DCM-67 Implementierungsabgrenzung und dokumentiere sie in `specs/004-dcm-67/spec.md`, `plan.md` und `tasks.md`
- [ ] T002 [P] Bestimme Zielbereiche der Umsetzung in `backend/src/`, `frontend/src/` und `specs/004-dcm-67/`
- [ ] T003 [P] Dokumentiere den Test-first-Workflow für DCM-67 in `specs/004-dcm-67/plan.md`

---

## Phase 2: Foundation

- [ ] T004 [P] Erstelle fehlschlagende Backend-Unit-Tests für Bereichserstellungsvalidierung in `backend/tests/Unit/Security/AreaCreationValidationTests.cs`
- [ ] T005 [P] Erstelle fehlschlagende Backend-Integrationstests für Kundenbereichsfilterung in `backend/tests/Integration/Security/CustomerAreaFilterTests.cs`
- [ ] T006 [P] Erstelle fehlschlagende Backend-Integrationstests für verbotene Bereichszugriffe in `backend/tests/Integration/Api/AreaAccessTests.cs`
- [ ] T007 [P] Erstelle fehlschlagende Backend-Integrationstests für Logging und Audit bei Bereichsaktionen in `backend/tests/Integration/Security/AreaAuditLoggingTests.cs`
- [ ] T008 Implementiere das Bereichs- und Zugriffsmodell (`Kunde`, `Bereich`, `Bereichsberechtigung`) in `backend/src/Domain/Security/`
- [ ] T009 Implementiere den Bereichsverwaltungsdienst und Autorisierungsentscheidungen in `backend/src/Application/Security/Areas/`
- [ ] T010 Implementiere strukturierte Logging-Helper für Bereichsereignisse in `backend/src/Infrastructure/Logging/AreaEventLogger.cs`
- [ ] T011 Implementiere Frontend-State für Bereichskachel-Liste, Startseitenstatus und Zugriffszustand in `frontend/src/app/core/auth/area-state.ts`

---

## Story 1 — Bereich/Kundenkontext anlegen (P1)

- [ ] T012 [P] Schreibe fehlschlagende Backend-Unit-Tests für Bereichserstellung mit gültigen Kundenkontexten in `backend/tests/Unit/Security/AreaCreationServiceTests.cs`
- [ ] T013 [P] Schreibe fehlschlagende Backend-Unit-Tests für Pflichtfeldvalidierung beim Bereichsanlegen in `backend/tests/Unit/Security/AreaCreationValidationTests.cs`
- [ ] T014 [P] Schreibe fehlschlagende Backend-Integrationstests für Bereichserstellungsendpunkte in `backend/tests/Integration/Api/AreaCreationControllerTests.cs`
- [ ] T015 [P] Schreibe fehlschlagende Frontend-Tests für Formularvalidierung und Admin-Zugriffsverhalten in `frontend/tests/area-creation/area-creation-form.spec.ts`
- [ ] T016 Implementiere `Kunde`- und `Bereich`-Entitäten und Mappings in `backend/src/Domain/Security/`
- [ ] T017 Implementiere Request/Response-DTOs für Bereichserstellung in `backend/src/Application/Security/Areas/`
- [ ] T018 Implementiere den Bereichserstellungs-Service in `backend/src/Application/Security/Areas/AreaCreationService.cs`
- [ ] T019 Implementiere Admin-only Bereichserstellungsendpunkt in `backend/src/Api/Controllers/AreaController.cs`
- [ ] T020 Implementiere Bereichserstellungs-Frontendformular in `frontend/src/app/features/admin/area-creation/area-creation.component.ts`
- [ ] T021 Implementiere API-Integration für Bereichserstellung in `frontend/src/app/features/admin/area-creation/area-creation.service.ts`
- [ ] T022 Implementiere Admin-Guard für Bereichsverwaltungsseiten in `frontend/src/app/core/auth/admin-access.guard.ts`

---

## Story 2 — Bereichsberechtigungen verwalten (P2)

- [ ] T023 [P] Schreibe fehlschlagende Backend-Unit-Tests für Bereichsberechtigungszuweisung und Isolierung in `backend/tests/Unit/Security/AreaPermissionServiceTests.cs`
- [ ] T024 [P] Schreibe fehlschlagende Backend-Integrationstests für Bereichsberechtigungsendpunkte in `backend/tests/Integration/Api/AreaPermissionControllerTests.cs`
- [ ] T025 [P] Schreibe fehlschlagende Frontend-Tests für Bereichsberechtigungs-UI in `frontend/tests/area-permissions/area-permission-management.spec.ts`
- [ ] T026 Implementiere `Bereichsberechtigung`-Entität und Mapping in `backend/src/Domain/Security/` und `backend/src/Infrastructure/Persistence/Configurations/`
- [ ] T027 Implementiere Request/Response-DTOs für Berechtigungsoperationen in `backend/src/Application/Security/Areas/`
- [ ] T028 Implementiere Bereichsberechtigungs-Service und Endpunkte in `backend/src/Application/Security/Areas/` und `backend/src/Api/Controllers/`
- [ ] T029 Implementiere Bereichsberechtigungs-Frontendverwaltung in `frontend/src/app/features/admin/area-permissions/`
- [ ] T030 Implementiere Logging für Berechtigungsänderungen in `backend/src/Application/Security/Areas/AreaPermissionService.cs`

---

## Story 3 — Kunden sehen nur freigegebene Bereiche (P3)

- [ ] T031 [P] Schreibe fehlschlagende Backend-Integrationstests für kundenfokussierte Bereichsliste in `backend/tests/Integration/Security/CustomerAreaListFilteringTests.cs`
- [ ] T032 [P] Schreibe fehlschlagende Backend-Integrationstests für verbotene Direktlink-Zugriffe in `backend/tests/Integration/Api/CustomerAreaAccessTests.cs`
- [ ] T033 [P] Schreibe fehlschlagende Frontend-Tests für Bereichskachel-Sichtbarkeitslogik in `frontend/tests/customer-access/customer-area-routes.spec.ts`
- [ ] T034 Implementiere kundenfokussierte Bereichsautorisierung in `backend/src/Security/CustomerAreaAccessPolicy.cs`
- [ ] T035 Setze Bereichsliste- und Detailendpoint-Filterung im Backend um in `backend/src/Api/Controllers/AreaController.cs`
- [ ] T036 Implementiere Kunden-Startseite mit Bereichskacheln in `frontend/src/app/features/customer-area/`
- [ ] T037 Implementiere Zugriff-verweigert-Seite oder Redirect für nicht autorisierte Bereichszugriffe in `frontend/src/app/shared/access-denied/`
- [ ] T038 Filtere die Navigation auf sichtbare Bereiche in `frontend/src/app/core/navigation/`

---

## Polish & Compliance

- [ ] T039 Dokumentiere das finale Abnahme- und Validierungsverfahren in `specs/004-dcm-67/quickstart.md`
- [ ] T040 Führe OWASP A01/A09-Review für neue Endpunkte und UI-Pfade durch und dokumentiere die Ergebnisse in `tasks.md`
- [ ] T041 Überprüfe strukturierte Logs für `area_created`, `permission_granted`, `permission_revoked` und `access_denied` in `backend/src/`
- [ ] T042 Führe Regressionstests und Build-Verifizierung für Backend und Frontend durch
- [ ] T043 Aktualisiere `tasks.md` nach Abschluss jeder Phase mit Status, Fortschritt und Blockern

---

## Notes

- Tests müssen vor Implementierung geschrieben werden.
- Bereiche dürfen in v1 genau einen Kundenkontext haben.
- Die Umsetzung folgt der bestehenden Auth- und Rollenstruktur aus `specs/001-rollenkonzept/`.
- Kein Scope Creep: Keine Favoriten, keine Mehrrollenverwaltung, keine Drag-and-Drop-Bereichsreihenfolge in dieser Phase.
