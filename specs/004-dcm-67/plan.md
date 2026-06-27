# Implementation Plan: DCM-67 Bereichs- und Kundenzugriffsverwaltung

**Branch**: `004-dcm-67` | **Date**: 2026-06-27 | **Spec**: [spec.md](./spec.md)

## Summary

Implementiere ein Bereichs- und Kundenzugriffsmodell, das autorisierten Administratoren erlaubt, neue Bereiche mit Kundenkontext anzulegen, getrennte Bereichsberechtigungen zu verwalten, Kunden nur freigegebene Bereiche anzuzeigen und eine Startseiten-/Navigationsoberfläche bereitzustellen.

Die Lösung bündelt die Anforderungen aus den Tickets `DCM-10`, `DCM-8`, `DCM-68`, `DCM-69` und `DCM-70` in einem konsolidierten Plan.

## Technical Context

- **Zielarchitektur**: Angular Frontend + .NET 10 Backend, PostgreSQL via Entity Framework
- **Umsetzungsschwerpunkt**: Bereichsverwaltung, kundenbezogene Autorisierung, sichere API-Endpunkte, klare Startseiten- und Navigationsdarstellung
- **Teststrategie**: Test-first; fehlende Backend- und Frontendtests müssen vor Implementierung angelegt und als fehlschlagend verifiziert werden
- **Deploy**: Docker-native Webapp, bereits bestehende Auth0-Integration bleibt bestehen
- **Constraints**: Kein zusätzliches Rollenmodell, keine Mehrfachkundenkontexte pro Bereich in v1, serverseitige Zugangsbeschränkung hat Vorrang vor Clientlogik

## Constitution Check

| Gate | Status | Plan-Response |
|------|--------|---------------|
| Spec-First Development | PASS | `spec.md` existiert und bildet die Autoritätsbasis. |
| Test-First | PASS | Plan fordert explizit fehlende Tests vor Implementierung. |
| Simplicity & YAGNI | PASS | Umsetzung beschränkt sich auf Bereichs- und Zugriffsverwaltung, keine Favoriten oder Mehrrollen. |
| Observability & Logging | PASS | Bereichsaktionen, Zugriffskontrolle und Fehlerpfade werden strukturiert geloggt. |
| Versioning & Breaking Changes | PASS | Backend-API-Änderungen werden als PATCH klassifiziert falls keine externe Contract-Änderung nötig ist. |
| OWASP A01 Broken Access Control | PASS | Bereichsautorisierung ist serverseitig und explizit. |
| OWASP A02 Cryptographic Failures | PASS | Keine neuen Secrets oder Kryptokonzepte eingeführt. |
| OWASP A03 Injection | PASS | Validierung von Eingaben beim Bereichsanlegen und bei Abfragen. |
| OWASP A04 Insecure Design | PASS | Default deny für nicht autorisierte Bereichszugriffe. |
| OWASP A05 Security Misconfiguration | PASS | Kein neuer Authentifizierungs- oder CORS-Pfad. |
| OWASP A06 Vulnerable Components | PASS | Keine neuen Abhängigkeiten geplant. |
| OWASP A07 Authentication Failures | PASS | Auth0 bleibt einzige Authentifizierungsquelle. |
| OWASP A08 Software & Data Integrity | PASS | Speicher- und Schemaänderungen werden zusammen committet. |
| OWASP A09 Logging & Monitoring Failures | PASS | Sicherheits- und Zugriffsereignisse werden ohne PII geloggt. |
| OWASP A10 SSRF | PASS | Keine externe URL-Verarbeitung durch Benutzerinput.

## Phase 0 Research

- Bestimme das minimal zulässige Bereichsmodell mit exakt einem Kundenkontext pro Bereich.
- Definiere den API-Vertrag für Bereichserstellung, Bereichsberechtigungen, Kundenbereichsliste und Startseitenaufhängung.
- Lege fest, wie Bereichsberechtigungen strukturiert im Backend modelliert werden.
- Beschreibe das gewünschte Verhalten bei nicht autorisierten Bereichszugriffen und Direktlinkversuchen.
- Kläre die minimale Beobachtbarkeit für Logs bei Erstellung, Änderung und Zugriffskontrolle.

## Phase 1 Design Outputs

- `research.md`: Entscheidungsschritte und Alternativen
- `data-model.md`: Entity-, DTO- und Berechtigungsmodelle
- `contracts/area-access-contract.md`: API- und UI-Verträge für Bereichs- und Kundenlogik
- `quickstart.md`: Implementierungs- und Validierungsfolge

## Phase 2 Implementation Plan

### Setup

- `T001` Dokumentation der User Stories und Akzeptanzkriterien in `specs/004-dcm-67/spec.md`
- `T002` Bestimme Implementierungsgrenzen für Backend-/Frontend-Dateien in bestehendem Projekt
- `T003` Lege eine einfache Branch-/Taskstruktur fest und dokumentiere sie in `tasks.md`

### Foundational

- `T004` Definiere Backend-DTOs und Commands für Bereichserstellung, Bereichsberechtigungen und Kundenbereichsliste
- `T005` Implementiere serverseitige Validierung der Bereichsanlage- und Zugriffsfelder
- `T006` Entwickle ein Bereichsautorisierungsmodell mit Kundenkontext und Bereichsberechtigungen
- `T007` Definiere strukturierte Logs für Bereichserstellung, Permisssion-Changelog und Zugriffsbeschränkungen
- `T008` Definiere Frontend-State für Bereichskacheln, Startseitenstatus und Fehlerzustände
- `T009` Baue eine Frontend-Service-API für Bereichsabfragen und Bereichsverwaltung

### Story 1 — Bereich/Kundenkontext anlegen (P1)

- `T010` Schreibe fehlschlagende Backendtests für Bereichserstellung und Validierung
- `T011` Schreibe fehlschlagende Frontendtests für Formularvalidierung und Zugriffsrechte
- `T012` Implementiere Bereichserstellungs-Endpunkt und Business-Logik
- `T013` Implementiere Frontend-Formular und API-Anbindung
- `T014` Implementiere Admin-Guard / Berechtigungsprüfung für Bereichsverwaltung

### Story 2 — Bereichsberechtigungen verwalten (P2)

- `T015` Schreibe fehlschlagende Tests für Berechtigungszuweisung und -isolation
- `T016` Implementiere Bereichsberechtigungsmodell und persistence mapping
- `T017` Implementiere Berechtigungsmanagement-Endpunkte und Logging
- `T018` Implementiere Frontend-UI für Berechtigungszuweisung und -verwaltung

### Story 3 — Kunden sehen nur freigegebene Bereiche (P3)

- `T019` Schreibe fehlschlagende Tests für customer-scoped Bereichsliste und Direktlink-Verweigerung
- `T020` Implementiere Kundenbereichsfilterung und Zugriffssperre im Backend
- `T021` Implementiere Frontend-Startseite mit Bereichskacheln und leeres Zustandsdesign
- `T022` Implementiere Zugriffsverweigerungsseiten / Navigationsflüsse bei fehlendem Bereichszugriff

### Polish & Delivery

- `T023` Überprüfe und dokumentiere den finalen End-to-End-Flow in `specs/004-dcm-67/quickstart.md`
- `T024` Führe eine OWASP-A01/A09-Prüfung für alle neuen Endpunkte und UI-Pfade durch
- `T025` Validieren, dass alle neuen Logs strukturiert und ohne PII ausgegeben werden
- `T026` Stelle sicher, dass alle Tests grünen und dokumentiere Ergebnisse in `tasks.md`

## Dependencies & Execution Order

- Foundational Tasks vor User Stories
- Story 1 zuerst für MVP-Fähigkeit
- Story 2 und Story 3 können auf der Foundation parallel umgesetzt werden
- Tests müssen vor Änderungen geschrieben werden

## Notes

- Dieses Repository speichert die Planungsartefakte; konkrete Implementierung sollte in der jeweiligen Anwendung erfolgen
- Berücksichtige bei der Umsetzung die bestehende Auth- und Rollenstruktur aus `specs/001-rollenkonzept`
- Alle neuen API-Verträge, die öffentlich konsumiert werden, sollten als Patch klassifiziert werden
