# Feature Specification: Rollenkonzept

**Feature Branch**: `001-rollenkonzept`  
**Created**: 2026-06-27  
**Status**: Draft  
**Input**: User description: "https://nexwork.atlassian.net/browse/DCM-5"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Rollen verwalten (Priority: P1)

Als Admin moechte ich Nutzer:innen eine der definierten Rollen zuweisen, damit jede Person genau die fuer ihre Aufgabe vorgesehenen Zugriffe und Aktionen erhaelt.

**Why this priority**: Ohne Rollenvergabe gibt es keine steuerbare Grundlage fuer Berechtigungen. Diese User Story schafft die fachliche Basis fuer alle weiteren Zugriffsregeln.

**Independent Test**: Kann eigenstaendig getestet werden, indem ein Admin mehreren Nutzer:innen unterschiedliche Rollen zuweist oder entzieht und die jeweils zugeordnete Rolle anschliessend eindeutig sichtbar ist.

**Acceptance Scenarios**:

1. **Given** eine Person mit Admin-Rechten und ein neuer Nutzer ohne Rollenzuweisung, **When** der Admin dem Nutzer die Rolle `Berater:in` zuweist, **Then** ist diese Rolle eindeutig als aktuelle Rolle des Nutzers hinterlegt.
2. **Given** eine Person mit Admin-Rechten und ein bestehender Nutzer mit der Rolle `Kund:in`, **When** der Admin die Rolle auf `Bewerter:in` aendert, **Then** ist nur noch die neue aktive Rolle sichtbar und wirksam.
3. **Given** ein Nutzer ohne Admin-Rechte, **When** dieser versucht, eine Rollenzuweisung zu aendern, **Then** wird die Aktion verhindert und eine verstaendliche Rueckmeldung angezeigt.

---

### User Story 2 - Eigene Inhalte sicher nutzen (Priority: P2)

Als Kund:in moechte ich nur meine eigenen fuer mich bestimmten Informationen und Funktionen sehen, damit meine Daten geschuetzt bleiben und ich mich auf meine eigenen Anliegen konzentrieren kann.

**Why this priority**: Diese User Story liefert unmittelbaren fachlichen Nutzen fuer die groesste Nutzergruppe und reduziert das Risiko unberechtigter Einsicht in fremde Inhalte.

**Independent Test**: Kann eigenstaendig getestet werden, indem sich eine Person mit der Rolle `Kund:in` anmeldet und nur ihre eigenen Informationen und keine fremden Inhalte angezeigt bekommt.

**Acceptance Scenarios**:

1. **Given** ein Nutzer mit der Rolle `Kund:in`, **When** er seine zugewiesenen Inhalte aufruft, **Then** sieht er nur Informationen und Funktionen, die fuer seine eigenen Anliegen freigegeben sind.
2. **Given** ein Nutzer mit der Rolle `Kund:in`, **When** er versucht, auf Inhalte eines anderen Kunden oder auf interne Verwaltungsbereiche zuzugreifen, **Then** wird der Zugriff verweigert und eine verstaendliche Rueckmeldung angezeigt.

---

### User Story 3 - Beratungs- und Bewertungszugriff abgrenzen (Priority: P3)

Als Berater:in oder Bewerter:in moechte ich genau die fuer meine Aufgabe erforderlichen Informationen sehen, damit ich meine Arbeit effizient erledigen kann, ohne unnoetige oder unzulaessige Zugriffe zu erhalten.

**Why this priority**: Diese User Story trennt operative Arbeit und unabhaengige Bewertung sauber voneinander und verhindert, dass fachliche Rollen mit administrativen Rechten vermischt werden.

**Independent Test**: Kann eigenstaendig getestet werden, indem ein Nutzer mit der Rolle `Berater:in` und ein Nutzer mit der Rolle `Bewerter:in` dieselben relevanten Bereiche aufrufen und jeweils nur die fuer ihre Rolle vorgesehenen Inhalte und Aktionen erhalten.

**Acceptance Scenarios**:

1. **Given** ein Nutzer mit der Rolle `Berater:in`, **When** er einen ihm fachlich zugeordneten Fall oeffnet, **Then** kann er die fuer die Beratung benoetigten Informationen sehen und im erlaubten Rahmen bearbeiten.
2. **Given** ein Nutzer mit der Rolle `Bewerter:in`, **When** er einen zu bewertenden Fall oeffnet, **Then** kann er die fuer die Bewertung benoetigten Informationen sehen, erhaelt jedoch keine administrativen Funktionen.
3. **Given** ein Nutzer mit der Rolle `Bewerter:in` oder `Berater:in`, **When** er versucht, Rollen zu vergeben oder Systembereiche fuer die Administration aufzurufen, **Then** wird der Zugriff verweigert und eine verstaendliche Rueckmeldung angezeigt.

---

### Edge Cases

- Was passiert, wenn ein Nutzer keine Rolle zugewiesen hat und versucht, geschuetzte Bereiche zu oeffnen?
- Wie verhaelt sich das System, wenn eine bestehende Rolle entzogen wird, waehrend der Nutzer noch aktiv arbeitet?
- Was passiert, wenn fuer eine Funktion keine fachlich freigegebene Rollenzuordnung definiert wurde?
- Wie wird verhindert, dass Nutzer:innen versehentlich Zugriff auf Bereiche erhalten, die mehreren Rollen aehnlich erscheinen, aber unterschiedliche Aufgaben abbilden?

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: Das System MUST die Rollen `Kund:in`, `Berater:in`, `Bewerter:in` und `Admin` als fachlich klar getrennte Rollen fuehren.
- **FR-002**: Das System MUST fuer jede der vier Rollen definieren, welche Bereiche eingesehen werden duerfen und welche Aktionen erlaubt sind.
- **FR-003**: Das System MUST sicherstellen, dass Nutzer:innen nur auf Inhalte und Aktionen zugreifen koennen, die ihrer aktuell zugewiesenen Rolle entsprechen.
- **FR-004**: Das System MUST sicherstellen, dass Kund:innen ausschliesslich ihre eigenen fuer sie bestimmten Informationen und Funktionen sehen und nutzen koennen.
- **FR-005**: Das System MUST sicherstellen, dass Berater:innen auf die fuer ihre Beratungstaetigkeit benoetigten Faelle und Informationen zugreifen und diese im erlaubten Rahmen bearbeiten koennen.
- **FR-006**: Das System MUST sicherstellen, dass Bewerter:innen auf die fuer die Bewertung erforderlichen Informationen zugreifen koennen, ohne administrative Rechte zu erhalten.
- **FR-007**: Das System MUST sicherstellen, dass nur Admins Rollen vergeben, aendern und entziehen koennen.
- **FR-008**: Das System MUST bei jeder Rollenvergabe und Rollenaenderung eindeutig ausweisen, welche Rolle einer Person aktuell zugeordnet ist.
- **FR-009**: Das System MUST Zugriffe auf nicht erlaubte Bereiche oder Aktionen verhindern und der betroffenen Person eine verstaendliche Rueckmeldung geben.
- **FR-010**: Das System MUST ein definiertes Verhalten fuer Nutzer:innen ohne gueltige Rollenzuweisung vorsehen und ihnen keinen unberechtigten Zugriff erlauben.
- **FR-011**: Das System MUST die vier Rollen fachlich so beschreiben, dass Hauptfunktionen eindeutig mindestens einer Rolle zugeordnet oder fuer sie ausgeschlossen werden koennen.
- **FR-012**: Das System MUST eine fachliche Grundlage bieten, auf der spaetere zusaetzliche Rollen oder Berechtigungen erweitert werden koennen, ohne die bestehenden vier Rollen in ihrer Bedeutung unklar zu machen.

### Key Entities *(include if feature involves data)*

- **Rolle**: Repräsentiert eine fachliche Berechtigungskategorie mit Name, erlaubten Bereichen und erlaubten Aktionen.
- **Nutzer:in**: Repräsentiert eine Person im System mit genau einer aktuell wirksamen Rollenzuweisung.
- **Rollenzuweisung**: Repräsentiert die Zuordnung einer Rolle zu einer Person inklusive aktuellem Status der Zuweisung.
- **Geschuetzter Bereich**: Repräsentiert einen Systembereich oder eine Funktion, fuer die der Zugriff anhand der zugewiesenen Rolle erlaubt oder verweigert wird.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Fuer 100 % der vier definierten Rollen liegt vor Start der Umsetzung eine freigegebene Beschreibung der erlaubten Zugriffe und Aktionen vor.
- **SC-002**: Fuer 100 % der priorisierten Hauptfunktionen laesst sich eindeutig bestimmen, welche der vier Rollen Zugriff hat, eingeschraenkten Zugriff hat oder keinen Zugriff hat.
- **SC-003**: In fachlichen Abnahmeszenarien koennen autorisierte Admins Rollen fuer Nutzer:innen in allen geprueften Kernfaellen ohne manuelle Sonderabsprachen vergeben oder aendern.
- **SC-004**: In fachlichen Abnahmeszenarien werden 100 % der geprueften unberechtigten Zugriffsversuche auf geschuetzte Bereiche oder Aktionen korrekt blockiert.
- **SC-005**: In fachlichen Abnahmeszenarien erhalten 100 % der geprueften Nutzer:innen bei verweigertem Zugriff eine verstaendliche Rueckmeldung statt eines stillen Fehlers oder unklaren Zustands.

## Assumptions

- Pro Nutzer:in ist fuer die erste Version genau eine aktuell wirksame Rolle vorgesehen.
- Das bestehende System verfuegt bereits ueber eine Form von Nutzerkonto oder Identitaet, auf die die Rollenzuweisung aufsetzen kann.
- Die vier im Jira-Ticket genannten Rollen bilden den verbindlichen Mindestumfang fuer die erste Version des Rollenkonzepts.
- Die fachliche Ausgestaltung einzelner Rechte innerhalb einer Rolle wird in der nachgelagerten Planung als Rechte-Matrix konkretisiert.
