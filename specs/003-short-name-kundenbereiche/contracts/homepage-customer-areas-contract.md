# API/UI Contract: Startseite Kundenbereiche

## Overview

This contract defines homepage behavior for authorized customer-area listing, empty/error state handling, navigation minimum scope, and search/pagination query semantics.

---

## Homepage Area List

### GET /api/home/customer-areas

**Description**: Returns customer areas visible to the current authenticated user for homepage rendering.

**Authorization**: Requires authentication and server-side customer-scope enforcement.

**Query Parameters**:

- `search` (optional): text term for area name filtering
- `page` (optional, default `1`): 1-based page index
- `pageSize` (optional, default server-defined): number of records per page

**Response 200 OK**:

```json
{
  "items": [
    {
      "areaId": "guid",
      "customerId": "guid",
      "areaName": "string",
      "areaStatus": "active",
      "navigationTarget": "/customer-areas/{areaId}"
    }
  ],
  "page": 1,
  "pageSize": 20,
  "totalItems": 135,
  "totalPages": 7,
  "isEmpty": false
}
```

**Response 200 OK (empty state)**:

```json
{
  "items": [],
  "page": 1,
  "pageSize": 20,
  "totalItems": 0,
  "totalPages": 0,
  "isEmpty": true
}
```

**Response 401 Unauthorized**:

```json
{
  "type": "authentication_error",
  "title": "Authentifizierung erforderlich",
  "detail": "You must be authenticated to access homepage customer areas."
}
```

**Response 403 Forbidden**:

```json
{
  "type": "authorization_error",
  "title": "Zugriff verweigert",
  "detail": "You do not have permission to access these customer areas.",
  "reasonCode": "wrong_scope|missing_role|inactive_user"
}
```

**Response 400 Bad Request**:

```json
{
  "type": "validation_error",
  "title": "Ungueltige Such- oder Paginierungsparameter",
  "detail": "Provided query parameters are invalid."
}
```

---

## Homepage Navigation Contract

### Required Navigation Entries

Frontend header/navigation for this feature must include at least:

- `home` -> `/`
- `customer-areas` -> `/customer-areas`

Any additional entries are optional and out of scope for this feature.

---

## Frontend Rendering Rules

1. If `isEmpty == false`, render tile list/grid from `items`.
2. If `isEmpty == true`, render text-based empty-state message.
3. If request fails with 4xx/5xx, render clear user-facing error state.
4. Search input updates query `search`; pagination controls update `page`/`pageSize`.
5. Tile selection navigates via `navigationTarget` only for returned authorized items.

---

## Observability Contract

Backend must emit structured events for each request outcome:

- `areas_loaded` with result count
- `areas_empty` when zero items are returned
- `areas_denied` for authorization failures
- `areas_error` for server failures

Minimal event fields:

- `requestId`
- `actorRef` (pseudonymized)
- `eventType`
- `resultCount` (where applicable)
- `reasonCode` (for denied/error)
- `occurredAt`

No raw secrets, tokens, or full personal identifiers may be logged.
