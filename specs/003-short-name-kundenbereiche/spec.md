# Feature Specification: Startseite Kundenbereiche

**Feature Branch**: `003-short-name-kundenbereiche`  
**Created**: 2026-06-27  
**Status**: Draft  
**Input**: User description: "Erstelle eine Startseite mit Kacheln von angelegten Kundenbereichen. Wenn keine Kacheln vorhanden sind, fuege ein neues GIF von einem tanzenden Elch hinzu. Nutze das vorhandene Nordmap-Logo und fuege eine Navigation hinzu."

## Clarifications

### Session 2026-06-27

- Q: Welchen Mindestumfang soll die Navigation auf der Startseite enthalten? → A: Startseite + Kundenbereiche.
- Q: Welches Verhalten soll bei vielen Kundenbereich-Kacheln gelten? → A: Suche und Paginierung.
- Q: Welche Quelle soll fuer das tanzende-Elch-GIF verwendet werden? → A: Aktuell kein GIF verwenden.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Kundenbereiche auf Startseite sehen (Priority: P1)

Als berechtigte Person moechte ich auf der Startseite alle fuer mich relevanten Kundenbereiche als Kacheln sehen, damit ich schnell in den richtigen Bereich wechseln kann.

**Why this priority**: Die Startseite ist der erste Einstiegspunkt und bestimmt, ob Nutzer den Kernnutzen sofort erreichen.

**Independent Test**: Kann eigenstaendig getestet werden, indem ein Nutzer mit mehreren Bereichen die Startseite aufruft und alle erwarteten Kacheln sieht und oeffnen kann.

**Acceptance Scenarios**:

1. **Given** es existieren mindestens zwei zugewiesene Kundenbereiche, **When** der Nutzer die Startseite aufruft, **Then** werden die Bereiche als einzelne, klar benannte Kacheln dargestellt.
2. **Given** die Kachelansicht ist sichtbar, **When** der Nutzer eine Kachel auswaehlt, **Then** gelangt er in den ausgewaehlten Kundenbereich.

---

### User Story 2 - Leere Startseite ohne GIF (Priority: P2)

Als berechtigte Person moechte ich bei fehlenden Kundenbereichen eine klare Leerseite ohne Medienpflicht sehen, damit ich erkenne, dass noch keine Bereiche angelegt wurden.

**Why this priority**: Ohne aussagekraeftige Leerseite wirkt die Anwendung fehlerhaft oder verwirrend.

**Independent Test**: Kann eigenstaendig getestet werden, indem ein Nutzer ohne zugewiesene Kundenbereiche die Startseite oeffnet und statt Kacheln eine Leerseite mit Hinweistexten sieht.

**Acceptance Scenarios**:

1. **Given** es sind keine Kundenbereiche verfuegbar, **When** die Startseite geladen wird, **Then** wird kein Kachelraster angezeigt und stattdessen die definierte Leerseite eingeblendet.
2. **Given** spaeter wird mindestens ein Kundenbereich verfuegbar, **When** die Startseite erneut geladen wird, **Then** verschwindet die Leerseite und die Kacheln werden angezeigt.

---

### User Story 3 - Orientierung ueber Logo und Navigation (Priority: P3)

Als Nutzer moechte ich auf der Startseite das Nordmap-Logo und eine klare Navigation sehen, damit ich mich schnell orientieren und zu wichtigen Bereichen wechseln kann.

**Why this priority**: Konsistente Markenfuehrung und Navigation reduzieren Fehlklicks und Einarbeitungsaufwand.

**Independent Test**: Kann eigenstaendig getestet werden, indem die Startseite auf Desktop und Mobile geoeffnet wird und Logo plus Navigation sichtbar und bedienbar sind.

**Acceptance Scenarios**:

1. **Given** die Startseite wird geladen, **When** der Seitenkopf angezeigt wird, **Then** ist das bereitgestellte Nordmap-Logo sichtbar.
2. **Given** die Navigation ist sichtbar, **When** der Nutzer einen Navigationseintrag auswaehlt, **Then** wird er zur Startseite oder in den Bereich Kundenbereiche gefuehrt.

---

### Edge Cases

- Wie verhaelt sich die Startseite, wenn ein Bereich kurz vor dem Laden entzogen oder geloescht wird?
- Was passiert, wenn der Leerseitenhinweis nicht geladen werden kann?
- Wie wird sichergestellt, dass bei sehr vielen Bereichen die Kacheln weiterhin auffindbar und bedienbar bleiben?
- Wie reagiert die Navigation bei ungueltigem Ziel oder fehlender Berechtigung?

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: Das System MUST auf der Startseite alle verfuegbaren Kundenbereiche als Kacheln darstellen.
- **FR-002**: Das System MUST pro Kachel mindestens den eindeutigen Bereichsnamen anzeigen.
- **FR-003**: Das System MUST dem Nutzer erlauben, ueber eine Kachel in den jeweiligen Kundenbereich zu wechseln.
- **FR-004**: Das System MUST bei null verfuegbaren Kundenbereichen eine Leerseite statt Kacheln anzeigen.
- **FR-005**: Die Leerseite MUST einen klaren Hinweistext enthalten, dass aktuell keine Kundenbereiche verfuegbar sind.
- **FR-006**: Das System MUST auf der Startseite das bereitgestellte Nordmap-Logo anzeigen.
- **FR-007**: Das System MUST eine sichtbare und bedienbare Navigation bereitstellen, die mindestens die Ziele Startseite und Kundenbereiche enthaelt.
- **FR-008**: Das System MUST nur Kundenbereiche anzeigen, fuer die der aktuelle Nutzer berechtigt ist.
- **FR-009**: Das System MUST bei Lade- oder Zugriffsproblemen eine verstaendliche, nutzerfreundliche Rueckmeldung anzeigen.
- **FR-010**: Die Startseite MUST auf Desktop und Mobile nutzbar bleiben.
- **FR-011**: Das System MUST bei vielen Kundenbereich-Kacheln eine Suche bereitstellen, damit Nutzer Bereiche schnell finden.
- **FR-012**: Das System MUST die Kachelmenge paginieren, damit die Startseite auch bei hoher Bereichsanzahl performant und bedienbar bleibt.

### Constitution Alignment *(mandatory)*

- **CA-001**: Vor Implementierung muessen Tests fuer Kachelanzeige bei vorhandenen Bereichen, Leerseite bei fehlenden Bereichen, sowie sichtbare Navigation vorhanden sein.
- **CA-002**: Backend muss strukturierte Logs fuer Startseiten-Datenabruf (Bereichsanzahl, Berechtigungsfilter, Fehlerfall) erzeugen; Frontend muss Fehler als klare Benutzerhinweise auf der Startseite ausgeben.
- **CA-003**: Es ist keine Aenderung oeffentlicher REST-Vertraege oder Datenbankschemata vorgesehen; erwarteter semantischer Impact ist Patch.
- **CA-004**: Relevante OWASP-Punkte sind Broken Access Control und Insecure Design; benoetigte Controls sind serverseitige Autorisierungspruefung pro Bereich, kein Client-only-Filtering und sichere Fehlerausgabe ohne interne Details.
- **CA-005**: Es sind keine neuen externen Abhaengigkeiten erforderlich; vorhandene UI- und Medienmechanismen sollen genutzt werden.

### Key Entities *(include if feature involves data)*

- **Kundenbereich-Kachel**: Repraesentiert einen zugreifbaren Bereichseintrag auf der Startseite mit Anzeigename und Zielbereich.
- **Startseiten-Navigationseintrag**: Repraesentiert einen verlinkbaren Navigationspunkt zu einem erlaubten Ziel.
- **Leerseitenzustand**: Repraesentiert den expliziten Zustand ohne verfuegbare Bereiche inklusive Hinweistext.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: In 100 % der Akzeptanztests werden bei verfuegbaren Bereichen die erwarteten Kacheln korrekt angezeigt.
- **SC-002**: In 100 % der Akzeptanztests wird bei null verfuegbaren Bereichen die Leerseite angezeigt.
- **SC-003**: In mindestens 95 % der Usability-Testfaelle finden Nutzer den gewuenschten Kundenbereich ueber die Startseite in unter 10 Sekunden.
- **SC-004**: In 100 % der Tests ist das Nordmap-Logo sichtbar und die Navigation bedienbar auf Desktop und Mobile.
- **SC-005**: In 100 % der Zugriffstests sieht ein Nutzer ausschliesslich die fuer ihn freigegebenen Kundenbereich-Kacheln.

## Assumptions

- Das Nordmap-Logo liegt bereits in der Anwendung bereit und darf auf der Startseite verwendet werden.
- Die Startseite nutzt bestehende Authentifizierungs- und Autorisierungskontexte fuer sichtbare Bereiche.
- Die Feature-Umsetzung umfasst keine Neudefinition des Rollenmodells, sondern nur die Darstellung bereits erlaubter Bereiche.

## Out of Scope

- Erstellung oder Pflege von Kundenbereichen auf der Startseite selbst.
- Neugestaltung des globalen Markenauftritts ausserhalb von Logoeinbindung und Navigation auf der Startseite.
- Erweiterte Personalisierung der Kachelreihenfolge (z. B. Favoriten, Drag-and-Drop).
