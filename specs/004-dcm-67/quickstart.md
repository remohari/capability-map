# Quickstart: DCM-67 Bereichs- und Kundenzugriffsverwaltung

## Ziel

Schnelle Inbetriebnahme der DCM-67 Dokumentation und Validierungsschritte ohne konkrete Implementierung. Diese Anleitung beschreibt die Reihenfolge, in der Tests, Designs und Verifikation angelegt werden.

## Empfohlene Schritte

1. Lege die Plan- und Task-Dateien an (erledigt).
2. Schreibe die folgenden Spezifikationsartefakte:
   - `research.md`
   - `data-model.md`
   - `contracts/area-management-contract.md`
3. Erstelle die ersten fehlschlagenden Tests in der App-Implementierung:
   - Backend: Bereichserstellung, Kundenbereichsfilterung, Zugangsbeschränkung
   - Frontend: Formularvalidierung, Bereichslisten-Sichtbarkeit, Zugriff verweigert
4. Implementiere schrittweise die Backend- und Frontendkomponenten gemäß Plan.
5. Führe eine vollständige Validierung durch:
   - Backend-Unit- und Integrationstests
   - Frontend-Testläufe oder manuelle Prüfungen
   - Linting/Type-Check
   - Audit-/Sicherheitsreview

## Akzeptanzprüfung

- Autorisierte Admins können einen Bereich mit Kundenkontext anlegen.
- Bereiche sind immer einem Kunden zugeordnet.
- Bereichsberechtigungen werden pro Bereich separat verwaltet.
- Kunden sehen nur die Bereiche, für die ihnen Zugriff gewährt wurde.
- Nicht freigegebene Bereiche sind nicht erreichbar und liefern eine klare Zugriffsfehler-Meldung.
- Alle neuen Bereichsereignisse werden strukturiert protokolliert.

## Prüfpunkte vor Implementierung

- `specs/004-dcm-67/spec.md` liegt vor und beschreibt User Stories, Anforderungen und Erfolgskriterien.
- `specs/004-dcm-67/plan.md` enthält die Umsetzungsstrategie und die Verfassungsprüfung.
- `specs/004-dcm-67/tasks.md` dokumentiert die erforderlichen Aufgaben und den Arbeitsfluss.
- `specs/004-dcm-67/research.md`, `data-model.md` und `contracts/area-management-contract.md` sind angelegt.

## Evaluationshinweise

- Die DCM-67 Artefakte sind Planungsdokumente: Implementierung erfolgt in der Anwendung, nicht hier.
- Test-first bedeutet, dass die Tests definiert und im Zielcode erstellt werden, bevor produktiven Code geändert wird.
- Dokumentiere Abweichungen in `research.md`, falls spätere Implementierungsentscheidungen von der ursprünglichen Annahme abweichen.
