# Feature Specification: Bereiche und kundenbezogene Zugriffsrechte

**Feature Branch**: `002-dcm-10`  
**Created**: 2026-06-27  
**Status**: Draft  
**Input**: User description: "https://nexwork.atlassian.net/browse/DCM-10"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Neuen Bereich fuer einen Kunden anlegen (Priority: P1)

Als berechtigte verwaltende Person moechte ich einen neuen Bereich fuer einen Kunden anlegen koennen, damit dieser Bereich als eigenstaendige Einheit verfuegbar ist und spaeter verwaltet werden kann.

**Why this priority**: Ohne das Anlegen eines Bereichs gibt es keine Grundlage fuer Verwaltung oder Zugriffssteuerung.

**Independent Test**: Kann eigenstaendig getestet werden, indem eine berechtigte Person einen neuen Bereich fuer einen Kunden erstellt und dieser anschliessend als eigener Bereich sichtbar ist.

**Acceptance Scenarios**:

1. **Given** eine berechtigte verwaltende Person und ein bestehender Kunde, **When** die Person einen neuen Bereich fuer diesen Kunden anlegt, **Then** wird der Bereich gespeichert und dem richtigen Kunden zugeordnet.
2. **Given** eine berechtigte verwaltende Person, **When** sie einen Bereich ohne erforderliche Angaben anlegen will, **Then** wird der Bereich nicht erstellt und eine verstaendliche Rueckmeldung angezeigt.

---

### User Story 2 - Bereich mit eigenen Berechtigungen verwalten (Priority: P2)

Als berechtigte verwaltende Person moechte ich pro Bereich separate Zugriffsberechtigungen vergeben und aendern koennen, damit jeder Bereich gezielt fuer die vorgesehenen Personen oder Kunden freigegeben werden kann.

**Why this priority**: Der zentrale fachliche Nutzen liegt in der getrennten Steuerung von Zugriffsrechten je Bereich.

**Independent Test**: Kann eigenstaendig getestet werden, indem fuer zwei verschiedene Bereiche unterschiedliche Berechtigungen gesetzt werden und diese getrennt erhalten bleiben.

**Acceptance Scenarios**:

1. **Given** zwei verschiedene Bereiche, **When** fuer jeden Bereich unterschiedliche Zugriffsrechte vergeben werden, **Then** gelten die Berechtigungen nur fuer den jeweils ausgewaehlten Bereich.
2. **Given** ein bestehender Bereich mit gesetzten Berechtigungen, **When** eine berechtigte Person die Freigaben aendert, **Then** werden die neuen Bereichsberechtigungen wirksam und die alten ersetzt.

---

### User Story 3 - Kunden auf eigene Bereiche begrenzen (Priority: P3)

Als Kunde moechte ich nur auf die Bereiche zugreifen koennen, die fuer mich freigegeben wurden, damit ich keine Daten oder Bereiche anderer Kunden einsehen kann.

**Why this priority**: Diese Abgrenzung schuetzt Kundendaten und verhindert unberechtigte Einsicht in fremde Bereiche.

**Independent Test**: Kann eigenstaendig getestet werden, indem ein Kunde mit Zugriff auf einen Bereich angemeldet wird und nur diesen Bereich, aber keine Bereiche anderer Kunden, sehen kann.

**Acceptance Scenarios**:

1. **Given** ein Kunde mit Freigabe fuer genau einen Bereich, **When** der Kunde die verfuegbaren Bereiche aufruft, **Then** sieht er nur den freigegebenen Bereich.
2. **Given** ein Kunde ohne Freigabe fuer einen anderen Kundenbereich, **When** der Kunde versucht, diesen Bereich aufzurufen, **Then** wird der Zugriff verweigert und eine verstaendliche Rueckmeldung angezeigt.

---

### Edge Cases

- Was passiert, wenn ein Bereich angelegt wird, der fuer denselben Kunden bereits in gleicher Form existiert?
- Wie verhaelt sich das System, wenn fuer einen Bereich noch keine Berechtigungen vergeben wurden?
- Was passiert, wenn einem Kunden der Zugriff auf einen Bereich entzogen wird, waehrend er den Bereich gerade verwendet?
- Wie wird verhindert, dass ein Kunde ueber Suche, Navigation oder Direktaufruf fremde Bereiche sieht?

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: Das System MUST das Anlegen eines neuen Bereichs fuer einen bestehenden Kunden ermoeglichen.
- **FR-002**: Das System MUST jeden neu angelegten Bereich eindeutig genau einem Kunden zuordnen.
- **FR-003**: Das System MUST einen angelegten Bereich anschliessend zur weiteren Verwaltung verfuegbar machen.
- **FR-004**: Das System MUST pro Bereich separate Zugriffsberechtigungen fuehren.
- **FR-005**: Das System MUST berechtigten verwaltenden Personen erlauben, Bereichsberechtigungen zu vergeben, zu aendern und zu entziehen.
- **FR-006**: Das System MUST sicherstellen, dass Zugriffsrechte eines Bereichs keine Freigabe fuer andere Bereiche bewirken.
- **FR-007**: Das System MUST sicherstellen, dass ein Kunde nur die Bereiche sehen und aufrufen kann, fuer die ihm eine Berechtigung erteilt wurde.
- **FR-008**: Das System MUST verhindern, dass ein Kunde Bereiche anderer Kunden einsehen kann, wenn dafuer keine ausdrueckliche Berechtigung besteht.
- **FR-009**: Das System MUST Zugriffsversuche auf nicht freigegebene Bereiche blockieren und dabei eine verstaendliche Rueckmeldung anzeigen.
- **FR-010**: Das System MUST das Anlegen eines Bereichs mit unvollstaendigen Pflichtangaben verhindern.
- **FR-011**: Das System MUST ein definiertes Verhalten fuer Bereiche ohne vergebene Berechtigungen vorsehen, sodass ohne Freigabe kein Kundenzugriff moeglich ist.

### Key Entities *(include if feature involves data)*

- **Kunde**: Repraesentiert eine Organisation oder Partei, fuer die ein oder mehrere Bereiche angelegt und getrennt abgesichert werden.
- **Bereich**: Repraesentiert eine verwaltbare Einheit innerhalb eines Kundenkontexts, fuer die eigene Zugriffsrechte gelten.
- **Bereichsberechtigung**: Repraesentiert die Freigabe, Sperre oder Zuordnung von Zugriffsrechten fuer einen konkreten Bereich.
- **Verwaltende Person**: Repraesentiert eine berechtigte Person, die Bereiche anlegt und Bereichsberechtigungen verwaltet.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: In 100 % der geprueften Fachtests kann eine berechtigte Person einen neuen Bereich fuer einen Kunden erfolgreich anlegen und dem richtigen Kunden zuordnen.
- **SC-002**: In 100 % der geprueften Fachtests bleiben Bereichsberechtigungen zwischen verschiedenen Bereichen sauber getrennt.
- **SC-003**: In 100 % der geprueften Fachtests sehen Kunden nur die fuer sie freigegebenen Bereiche.
- **SC-004**: In 100 % der geprueften Fachtests werden Zugriffsversuche auf nicht freigegebene Bereiche blockiert.
- **SC-005**: In 100 % der geprueften Fachtests erhalten Nutzer bei abgelehnten Zugriffen oder unvollstaendigen Eingaben eine verstaendliche Rueckmeldung.

## Assumptions

- Es existieren bereits Kunden im System, denen neue Bereiche zugeordnet werden koennen.
- Es gibt bereits mindestens eine fachlich berechtigte verwaltende Rolle, die Bereiche anlegen und Berechtigungen pflegen darf.
- Ein Bereich gehoert in der ersten Version genau einem Kunden.
- Die Spezifikation behandelt die fachliche Trennung von Bereichszugriffen; weitergehende interne Rollenmodelle werden in der Planungsphase konkretisiert.
