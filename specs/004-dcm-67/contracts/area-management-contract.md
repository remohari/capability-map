# Contract: DCM-67 Bereichs- und Kundenzugriffsverwaltung

## Overview

Dieser Contract definiert die wesentlichen API- und UI-Verträge für Bereichsverwaltung, Kundenkontextzuordnung und Bereichs-Zugriffssteuerung.

## API-Verträge

### Bereich erstellen

- **Endpoint**: `POST /api/areas`
- **Request**:
  - `customerId` oder vollständige Kundendaten
  - `areaName`
  - `description` optional
- **Response**:
  - `areaId`
  - `customerId`
  - `areaName`
  - `createdAt`
- **Fehler**:
  - `400` bei Validierungsfehlern
  - `409` bei doppelten Bereichen
  - `403` bei fehlender Admin-Berechtigung

### Bereichsberechtigung vergeben

- **Endpoint**: `POST /api/areas/{areaId}/permissions`
- **Request**:
  - `userId`
  - `roleName`
  - `grantedBy`
  - `expiresAt` optional
- **Response**:
  - `permissionId`
  - `areaId`
  - `userId`
  - `roleName`
  - `grantedAt`
- **Fehler**:
  - `400` bei Validierung
  - `404` wenn `areaId` nicht existiert
  - `403` bei fehlender Admin-Berechtigung

### Kundenbereichs-Liste

- **Endpoint**: `GET /api/customers/{customerId}/areas`
- **Query**:
  - `searchTerm` optional
  - `page` optional (default 1)
  - `pageSize` optional (default 20)
- **Response**:
  - `items`: authorized area summary objects
  - `totalItems`
  - `totalPages`
  - `page`
  - `pageSize`
  - `isEmpty`
- **Fehler**:
  - `403` wenn der Nutzer keinen Zugriff auf den Kundenbereich hat

### Bereich-Detailzugriff

- **Endpoint**: `GET /api/areas/{areaId}`
- **Response**:
  - `areaId`
  - `customerId`
  - `areaName`
  - `description`
  - `status`
- **Fehler**:
  - `403` bei fehlender Bereichsberechtigung
  - `404` bei nicht vorhandenem Bereich

## UI-Verträge

### Admin-Bereichsverwaltung

- Admins sehen eine Oberfläche zum Anlegen neuer Bereiche mit Kundenzuordnung.
- Die Eingaben müssen Pflichtfelder validieren und im Fehlerfall verständliche Meldungen anzeigen.
- Das UI enthält eine Liste existierender Bereiche und eine Möglichkeit zur Berechtigungsverwaltung pro Bereich.

### Kundenbereichs-Startseite

- Kunden sehen nur Bereiche, für die sie explizit berechtigt wurden.
- Bei keinen sichtbaren Bereichen wird ein erläuternder Leerzustand angezeigt.
- Direkter Zugriff auf nicht berechtigte Bereiche führt zu einer Zugriff-verweigert-Seite oder klarer Fehlernachricht.

### Navigation

- Es gibt mindestens einen Menüpunkt für `Home` und einen für `Customer Areas`.
- Die Navigation respektiert das Zugriffsmodell und zeigt nur erlaubte Ziele als aktive Links.

## Observability Contract

- Backend-Events müssen folgende Ereignistypen abbilden:
  - `area_created`
  - `permission_granted`
  - `permission_revoked`
  - `access_denied`
- Ereignisse müssen `actorRef`, `subjectRef`, `outcome` und `occurredAt` enthalten.
- Sensible Daten dürfen nicht roh geloggt werden.

## Security Contract

- Alle Bereichsoperationen erfordern Authentifizierung.
- Bereichsanhlege- und Berechtigungsendpunkte erfordern Admin-Berechtigung.
- Kundenbereichslisten und Bereichszugriffe erfordern explizite Bereichsberechtigung und passenden Kundenkontext.
- Backend-Prüfung ist die Quelle der Wahrheit; die UI darf nur visualisieren.
