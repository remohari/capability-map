# Feature Specification: DCM-67 Bereichs- und Kundenzugriffsverwaltung

**Feature Branch**: `004-dcm-67`
**Created**: 2026-06-27
**Status**: Draft
**Input**: User description: "https://nexwork.atlassian.net/browse/DCM-67"

Diese Spezifikation adressiert die übergreifende Umsetzung von Bereichs- und Kundenzugriffsanforderungen, wie sie in den zugehörigen Jira-Tickets beschrieben sind:
- `DCM-10`
- `DCM-8`
- `DCM-68`
- `DCM-69`
- `DCM-70`

## Clarifications

- Diese Spezifikation bündelt mehrere fachlich zusammenhängende Tickets unter dem Dach von DCM-67.
- Der Fokus liegt auf der Anlage neuer Bereiche/Kundenkontexte, der Verwaltung separater Bereichsberechtigungen, der Sichtbarkeit nur freigegebener Bereiche für Kunden sowie auf der zugehörigen Startseiten-/Navigations-Darstellung.
- Detaillierte Jira-Details sind bei Bedarf im Plan oder in Tasks weiter zu spezifizieren.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Bereich/Kundenkontext anlegen (Priority: P1)

Als verwaltende Person möchte ich einen neuen Bereich mit zugehörigem Kundenkontext anlegen können, damit dieser als eigenständige Verwaltungseinheit verfügbar ist und später gezielt Berechtigungen erhalten kann.

**Why this priority**: Ohne die Anlage neuer Bereiche und Kundenkontexte ist das gesamte Zugriffsmodell nicht nutzbar.

**Independent Test**: Testen, dass eine berechtigte Person einen Bereich mit Kundenkontext anlegt und dieser in der Bereichsliste korrekt erscheint.

**Acceptance Scenarios**:

1. **Given** eine berechtigte verwaltende Person, **When** sie einen neuen Bereich mit Kundenangaben anlegt, **Then** wird der Bereich gespeichert und einem Kundenkontext zugeordnet.
2. **Given** ein Bereich wird mit unvollständigen Pflichtangaben angelegt, **When** die Speicherung ausgelöst wird, **Then** wird die Anlage verhindert und eine verständliche Rückmeldung angezeigt.
3. **Given** ein Kunde existiert bereits, **When** ein neuer Bereich für diesen Kunden angelegt wird, **Then** wird der bestehende Kundenkontext wiederverwendet und der Bereich korrekt zugeordnet.

---

### User Story 2 - Bereichsberechtigungen verwalten (Priority: P2)

Als verwaltende Person möchte ich pro Bereich eigene Zugriffsberechtigungen vergeben, ändern und entziehen können, damit jeder Bereich nur für die vorgesehenen Personen oder Kundengruppen verfügbar ist.

**Why this priority**: Die fachliche Trennung und Sicherheit der Bereiche hängt von getrennten Berechtigungen ab.

**Independent Test**: Testen, dass ein Bereich geöffnet und unterschiedliche Berechtigungen für mehrere Rollen bzw. Nutzergruppen gespeichert werden können.

**Acceptance Scenarios**:

1. **Given** zwei Bereiche, **When** für jeden Bereich unterschiedliche Zugriffsregeln gesetzt werden, **Then** gelten die Regeln nur für den jeweiligen Bereich.
2. **Given** ein bestehender Bereich, **When** die Zugriffsberechtigungen geändert werden, **Then** werden die neuen Regeln sofort wirksam.
3. **Given** einem Bereich werden alle Zugriffsberechtigungen entzogen, **When** ein Kunde versucht, den Bereich zu öffnen, **Then** wird der Zugriff verweigert.

---

### User Story 3 - Kunden nur freigegebene Bereiche sehen (Priority: P3)

Als Kunde möchte ich nur die Bereiche sehen und aufrufen können, für die mir explizit Zugriff gewährt wurde, damit ich keine Daten anderer Kunden einsehen kann.

**Why this priority**: Datenschutz und Zugriffssegregation sind zentrale Qualitätsanforderungen.

**Independent Test**: Testen, dass ein Kunde nur in seinem Berechtigungsumfang sichtbare Bereiche sieht und andere Bereiche nicht erreichbar sind.

**Acceptance Scenarios**:

1. **Given** ein Kunde hat Zugriff auf genau einen Bereich, **When** er die Bereichsliste aufruft, **Then** sieht er nur diesen Bereich.
2. **Given** ein Kunde versucht, einen nicht freigegebenen Bereich per Direktlink aufzurufen, **Then** wird der Zugriff verweigert und eine klare Meldung angezeigt.
3. **Given** ein Bereich für den Kunden später freigegeben wird, **When** die Bereichsliste aktualisiert wird, **Then** erscheint der Bereich.

---

### User Story 4 - Startseite und Navigation für Bereiche (Priority: P3)

Als Nutzer möchte ich auf einer Startseite die mir verfügbaren Bereiche als Kacheln sehen und eine klare Navigation nutzen können, damit ich schnell zu meinen relevanten Bereichen gelange.

**Why this priority**: Benutzerführung und Auffindbarkeit der Bereiche verbessern die Nutzbarkeit des Systems.

**Independent Test**: Testen, dass die Startseite eine Kachelansicht der autorisierten Bereiche anzeigt und ein Navigationsziel zu den Bereichen bereitstellt.

**Acceptance Scenarios**:

1. **Given** ein Nutzer mit mindestens einem berechtigten Bereich, **When** er die Startseite öffnet, **Then** sieht er diesen Bereich als Kachel.
2. **Given** ein Nutzer ohne berechtigte Bereiche, **When** er die Startseite öffnet, **Then** sieht er einen erklärenden Leerzustand ohne Kacheln.
3. **Given** die Startseite wird aufgerufen, **When** der Nutzer die Navigation nutzt, **Then** führt sie zu den relevanten Bereichsansichten.

---

## Edge Cases

- Was passiert, wenn ein Bereich für denselben Kunden zweimal angelegt werden soll?
- Wie verhält sich das System, wenn einem Kunde der Zugriff auf einen Bereich entzogen wird, während er ihn offen hat?
- Was passiert, wenn temporär keine Bereiche für einen Nutzer verfügbar sind?
- Wie wird sichergestellt, dass Direktlinks zu nicht freigegebenen Bereichen blockiert werden?
- Was passiert, wenn ein Bereich ohne zugewiesene Berechtigungen existiert?

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: Das System MUST das Anlegen eines neuen Bereichs mit Kundenkontext ermöglichen.
- **FR-002**: Das System MUST einen neuen Bereich eindeutig einem bestehenden oder neu anzulegenden Kundenkontext zuordnen.
- **FR-003**: Das System MUST pro Bereich getrennte Zugriffsberechtigungen führen.
- **FR-004**: Das System MUST Berechtigungen für Bereiche hinzufügen, ändern und entziehen können.
- **FR-005**: Das System MUST sicherstellen, dass Bereichsberechtigungen nicht automatisch auf andere Bereiche übertragen werden.
- **FR-006**: Das System MUST Kunden nur die Bereiche anzeigen, für die sie freigegeben sind.
- **FR-007**: Das System MUST den Zugriff auf nicht freigegebene Bereiche blockieren.
- **FR-008**: Das System MUST einen Startseitenstatus für vorhandene und fehlende Bereiche anzeigen.
- **FR-009**: Das System MUST bei unvollständigen Pflichtangaben beim Bereichsanlegen eine klare Validierungsfehlermeldung anzeigen.
- **FR-010**: Das System MUST die Bereichsverwaltung für berechtigte Personen zugänglich machen.
- **FR-011**: Das System MUST sicherstellen, dass Nutzer mit geringeren Rechten keine administrativen Bereichsaktionen durchführen können.
- **FR-012**: Das System MUST Navigationsziele für verfügbare Bereiche bereitstellen.

### Constitution Alignment *(mandatory)*

- **CA-001**: Vor Implementierung müssen Tests für Bereichsanlage, Zugriffsbeschränkung und Startseitenanzeige vorhanden sein.
- **CA-002**: Neue Backendpfade müssen strukturierte Logs für Bereichsoperationen und Autorisierungsprüfungen erzeugen.
- **CA-003**: Fehler- und Zugriffsablehnungen müssen dem Nutzer verständlich dargestellt werden.
- **CA-004**: Der semantische Impact wird als Patch eingestuft, sofern keine öffentliche API-/Schemaveränderung erforderlich ist.
- **CA-005**: Relevante OWASP-Punkte sind A01 Broken Access Control und A04 Insecure Design. Die Kontrollen sind serverseitige Bereichsautorisierung und klare Fehlerzustände.

### Key Entities *(include if feature involves data)*

- **Kunde**: Eine Organisation oder Partei, deren Kontext einem Bereich zugeordnet wird.
- **Bereich**: Eine verwaltbare Einheit mit eigenem Kundenkontext und eigenen Berechtigungen.
- **Bereichsberechtigung**: Eine Zuordnung, die steuert, welche Nutzer oder Rollen Zugriff auf einen Bereich haben.
- **Startseitenzustand**: Repräsentiert die Anzeige verfügbarer Bereiche oder einen Leerzustand für den Nutzer.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: In 100 % der Abnahmetests kann ein berechtigter Nutzer einen neuen Bereich mit Kundenkontext anlegen.
- **SC-002**: In 100 % der Tests bleiben Bereichsberechtigungen zwischen Bereichen separat.
- **SC-003**: In 100 % der Tests sehen Kunden nur die für sie freigegebenen Bereiche.
- **SC-004**: In 100 % der Tests werden nicht freigegebene Bereiche für unberechtigte Nutzer korrekt blockiert.
- **SC-005**: In 100 % der Tests erhält ein Nutzer bei verweigertem Zugriff eine verständliche Fehlermeldung.
- **SC-006**: In 100 % der Tests zeigt die Startseite bei vorhandenen Bereichen eine Kachelansicht und bei fehlenden Bereichen einen erklärenden Leerzustand.

## Assumptions

- Es existiert ein Nutzerkonto- und Authentifizierungsmodell, auf dem die Bereichsrechte aufbauen.
- Es gibt eine berechtigte verwaltende Rolle, die Bereiche und Berechtigungen pflegen darf.
- Die Tickets DCM-10, DCM-8, DCM-68, DCM-69 und DCM-70 werden im Rahmen von DCM-67 fachlich gemeinsam umgesetzt.
- Die ersten Ausbaustufen benötigen keine zusätzlichen Rollenmodelle über die bestehende Berechtigungsstruktur hinaus.

## Out of Scope

- Komplexe Mehrkundenstrukturen mit mehreren gleichzeitigen Kundenkontexten pro Bereich.
- Erweiterte Personalisierung der Startseitenbereiche wie Favoriten oder Drag-and-Drop.
- Umfangreiche Rollenmodelle jenseits der für Bereichsberechtigungen benötigten Grundfunktionen.
