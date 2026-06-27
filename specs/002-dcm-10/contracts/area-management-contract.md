# API Contract: Bereich-Management und kundenbezogene Zugriffsrechte

## Overview

This contract defines the API surface for area creation, permission management, and customer-scoped access enforcement. All endpoints require authentication and appropriate role-based authorization.

---

## Area Creation

### POST /api/areas

**Description**: Create a new area with an associated customer context.

**Authorization**: Requires `Admin` role or equivalent administrative capability.

**Request Body**:

```json
{
  "customerName": "string (required)",
  "customerIdentifier": "string (required)",
  "areaName": "string (required)",
  "areaDescription": "string (optional)"
}
```

**Response 201 Created**:

```json
{
  "areaId": "guid",
  "customerId": "guid",
  "name": "string",
  "description": "string",
  "status": "active",
  "createdAt": "ISO 8601 timestamp"
}
```

**Response 400 Bad Request** (validation error):

```json
{
  "type": "validation_error",
  "title": "Unvollständige oder ungültige Eingaben",
  "details": [
    {
      "field": "areaName",
      "message": "Area name is required"
    }
  ]
}
```

**Response 409 Conflict** (duplicate area):

```json
{
  "type": "conflict",
  "title": "Bereich existiert bereits",
  "detail": "An area with this name already exists for the specified customer."
}
```

**Response 403 Forbidden** (insufficient permissions):

```json
{
  "type": "authorization_error",
  "title": "Zugriff verweigert",
  "detail": "You do not have permission to create areas."
}
```

---

## Area List (Customer-Scoped)

### GET /api/areas

**Description**: List areas visible to the current user, filtered by customer scope and area permissions.

**Authorization**: Requires authentication and a valid role.

**Query Parameters**:

- `customerId` (optional): Filter by specific customer

**Response 200 OK**:

```json
[
  {
    "areaId": "guid",
    "customerId": "guid",
    "customerName": "string",
    "name": "string",
    "description": "string",
    "status": "active"
  }
]
```

**Response 401 Unauthorized**:

```json
{
  "type": "authentication_error",
  "title": "Authentifizung erforderlich",
  "detail": "You must be authenticated to view areas."
}
```

---

## Area Details

### GET /api/areas/{areaId}

**Description**: Get details of a specific area, only if the user has access.

**Authorization**: Requires authentication and area permission.

**Response 200 OK**:

```json
{
  "areaId": "guid",
  "customerId": "guid",
  "customerName": "string",
  "name": "string",
  "description": "string",
  "status": "active",
  "permissions": [
    {
      "userId": "guid",
      "roleName": "string",
      "grantedAt": "ISO 8601 timestamp"
    }
  ]
}
```

**Response 403 Forbidden**:

```json
{
  "type": "authorization_error",
  "title": "Zugriff verweigert",
  "detail": "You do not have permission to view this area."
}
```

---

## Area Permission Management

### POST /api/areas/{areaId}/permissions

**Description**: Grant or revoke area permissions for a specific user.

**Authorization**: Requires `Admin` role or equivalent administrative capability.

**Request Body**:

```json
{
  "userId": "guid (required)",
  "action": "grant|revoke (required)",
  "changeReason": "string (optional)"
}
```

**Response 200 OK**:

```json
{
  "permissionId": "guid",
  "areaId": "guid",
  "userId": "guid",
  "action": "grant|revoke",
  "grantedAt": "ISO 8601 timestamp",
  "grantedBy": "string",
  "changeReason": "string"
}
```

**Response 403 Forbidden**:

```json
{
  "type": "authorization_error",
  "title": "Zugriff verweigert",
  "detail": "You do not have permission to manage permissions for this area."
}
```

---

## Access Denial Behavior

### General 403 Response

All access-denied responses follow a consistent structure:

```json
{
  "type": "authorization_error",
  "title": "Zugriff verweigert",
  "detail": "You do not have permission to access this resource.",
  "reasonCode": "missing_role|wrong_role|wrong_scope|no_area_permission|customer_mismatch"
}
```

**Logging Requirements**:

- Every 403 response must trigger a structured audit log entry with:
  - `eventType`: `access_denied`
  - `actorRef`: pseudonymized user reference
  - `target`: area ID or action key
  - `reasonCode`: structured reason
  - `occurredAt`: timestamp

---

## Error Handling

### Validation Errors (400)

All validation errors follow the problem details format:

```json
{
  "type": "validation_error",
  "title": "Validation failed",
  "status": 400,
  "details": [
    {
      "field": "fieldName",
      "message": "Human-readable error message"
    }
  ]
}
```

### Internal Errors (500)

```json
{
  "type": "internal_error",
  "title": "Interner Fehler",
  "detail": "An unexpected error occurred. Please try again later."
}
```
