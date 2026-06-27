# Research: DCM-67 Bereichs- und Kundenzugriffsverwaltung

## Ziel

Die Recherche dokumentiert die wichtigsten Designentscheidungen für Bereichs- und Kundenzugriffsverwaltung in Phase 1. Sie klärt, wie Kundenkontext, Bereichsberechtigungen, Kundenfluchtlinien und Startseitenverhalten zusammenwirken.

## Decision 1: Bereichsmodell bleibt einfach und ein-eindeutig

- Jeder Bereich (`Bereich`) wird mit genau einem Kundenkontext (`Kunde`) verknüpft.
- Eine Kombination aus `Kunde` und Bereichsname muss unique sein, um doppelte Bereiche zu verhindern.
- Das System erlaubt in v1 keine Bereiche ohne Kundenbindung.

**Begründung**: Eine feste Zuordnung verhindert unklare Zugriffsszenarien und entspricht der Anforderung, dass Bereiche kundenbezogene Kontexte besitzen.

## Decision 2: Bereichsberechtigungen sind von Rollen getrennt

- Bereichsberechtigungen (`Bereichsberechtigung`) werden separat vom bestehenden Rollenmodell geführt.
- Ein Nutzer braucht sowohl eine gültige Rolle als auch eine explizite Bereichsberechtigung, um Zugriff auf einen Bereich zu erhalten.

**Begründung**: Dadurch bleibt die bestehende Rollenarchitektur stabil und die Anforderungen an die Trennung von Bereichsrechten werden erfüllt.

## Decision 3: Serverseitige Kunden- und Bereichsprüfung ist zwingend

- Frontend-Filtering dient nur zur UX; echte Zugriffssperre wird immer im Backend geprüft.
- Sogar bei direktem Linkversuch muss ein Bereichspass durch die serverseitige Autorisierungslogik.

**Begründung**: Sicherheitsrelevante Zugriffsgrenzen dürfen nicht rein im Client gesetzt werden. Dies entspricht dem OWASP A01-Prinzip.

## Decision 4: Strukturierte Audit-Logs für Bereichsaktionen

- Backend-Events müssen structured logs für mindestens folgende Ereignistypen erzeugen:
  - `area_created`
  - `permission_granted`
  - `permission_revoked`
  - `access_denied`
- Logs dürfen keine unnötigen PII-Felder enthalten.

**Begründung**: Bereichsaktionen und Zugriffskontrollen sind sicherheitsrelevant und müssen nachvollziehbar bleiben.

## Decision 5: Bereiche ohne Rechte sind standardmäßig gesperrt

- Ein Bereich gilt als geschlossen, solange keine explizite Berechtigung vorhanden ist.
- Das Default-Verhalten ist `deny all` für Kunden.

**Begründung**: Das schützt vor unbeabsichtigter Freigabe neuer Bereiche und hält sich an das Least-Privilege-Prinzip.

## Decision 6: Fehlerfälle und Duplikate klar behandeln

- Bereichsanlage mit fehlenden Pflichtdaten wird abgelehnt und liefert klare Validierungsfehler.
- Ein Versuch, einen Bereich für denselben Kunden doppelt anzulegen, wird als Fehler zurückgewiesen.

**Begründung**: Das reduziert Inkonsistenzen und unterstützt sichere Betriebsprozesse.

## Alternativen, die verworfen wurden

- Mehrere Kundenkontexte pro Bereich: zu komplex für v1.
- Berechtigungsverwaltung nur über Rollen: widerspricht der Anforderung separater Bereichsrechte.
- Client-side only Filtering: unsicher und deshalb nicht akzeptabel.
