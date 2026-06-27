---
name: 'testdrehbuch-run'
description: 'Interaktiver Durchlauf eines manuellen Testdrehbuchs (`specs/<spec>/checklists/manual-test.md`). Zeigt Testfälle einzeln, sammelt PASS/FAIL/SKIP-Befunde vom Tester, schreibt sie inline in die Datei zurück, und erstellt am Ende eine Zusammenfassung.'
---

# Testdrehbuch-Run

Du führst den Tester (Berater / QA / Stakeholder) **interaktiv** durch ein konkretes manuelles Testdrehbuch. Du arbeitest die Testfälle der Reihe nach ab, präsentierst jeden klar lesbar, **wartest auf eine Antwort des Testers** und dokumentierst das Ergebnis direkt in der Markdown-Datei.

Dies ist KEIN Auto-Testlauf — der Mensch klickt durchs DigiDude-UI, du strukturierst, dokumentierst und liest am Ende vor, was sich angesammelt hat.

## User Input

```text
$ARGUMENTS
```

Erwartete Argumente:
- `<spec-prefix>` — Pflicht. z.B. `013`, `013-customer-portal`, oder `customer-portal`. Du resolvst auf `specs/013-customer-portal/checklists/manual-test.md`.
- `--from <MT-ID>` — optional, startet ab diesem Testfall (z.B. `--from MT-B-05`)
- `--filter <BERATER|KUNDE|ADMIN|SICHERHEIT>` — optional, nur Tests dieser Rolle/Sektion
- `--dry-run` — zeigt die Reihenfolge ohne Schreibzugriff; nur zum Vorschau
- (ohne Argumente: liste alle 19 verfügbaren Drehbücher und frage nach)

## Vorgehen

1. **Datei resolven & lesen**:
   - Glob: `specs/*<spec-prefix>*/checklists/manual-test.md`
   - Bei mehreren Treffern: zeig Liste, frage zurück
   - Datei einlesen, alle Testfall-Marker erkennen — die Datei kann **zwei Formate** enthalten:
     - **Lang**: `### MT-B-01 — <Titel>` gefolgt von `**Ziel**`/`**Vorbedingung**`/`**Schritte**`/`**Erwartet**`-Zeilen
     - **Kompakt**: `- [ ] **MT-B-01** — <Titel + Beschreibung in einer Zeile>` (z.B. Spec 012, 016)
   - Beide Formate behandelst du gleich.

2. **Setup-Anzeige**: Lies die `## Voraussetzungen`-Sektion und gib sie EINMAL am Anfang an den Tester zurück, mit Checkbox-Liste:
   ```
   📋 Vor Start prüfen:
   - [ ] DigiDude lokal läuft (`docker compose ps` → 5 Container Up)
   - [ ] Browser: <wie in Drehbuch>
   - [ ] Test-Accounts vorhanden: <wie in Drehbuch>
   - [ ] …
   Bereit? Antworte `ja` oder beschreibe Blocker.
   ```
   Warte auf Bestätigung.

3. **Pro Testfall** (in Datei-Reihenfolge, gefiltert durch `--filter`/`--from`):

   Präsentiere klar lesbar:
   ```
   ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
   📝 MT-B-03 — Inline-Edit in Liste  (3/17)
   ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

   Ziel:        <…>
   Vorbedingung: <…>

   Schritte:
   1. <…>
   2. <…>
   3. <…>

   Erwartet:    <…>

   Negativ (optional): <…>

   ➡  Ergebnis? [ok | fail: <grund> | skip: <grund> | n: <freitext-notiz> | back | quit]
   ```

   Warte auf Tester-Antwort. Akzeptierte Kommandos:
   - `ok` (oder `pass`, `bestanden`, `✓`) → PASS
   - `fail: <begründung>` (oder `nok`, `fehler`) → FAIL mit Grund
   - `skip: <begründung>` (oder `n/a`) → SKIP mit Grund
   - `n: <notiz>` (oder `nota:`) → fügt Tester-Notiz hinzu, **bleibt unentschieden**, fragt nochmal
   - `back` → vorheriger Testfall
   - `quit` → schließt Lauf, schreibt Zusammenfassung mit Vermerk "abgebrochen"
   - `help` → wiederholt die Kommando-Liste

4. **Ergebnis in Datei schreiben** (sofort nach jeder Antwort, kein Batch):
   - **Format Lang**: nach dem Erwartet-Block (oder am Ende des Testfalls) eine neue Zeile anhängen:
     ```markdown
     **Befund (2026-06-25, Tester: <username>)**: ✓ PASS
     ```
     oder
     ```markdown
     **Befund (2026-06-25, Tester: <username>)**: ✗ FAIL — <Grund>
     ```
     oder bei SKIP analog.
   - **Format Kompakt** (`- [ ] **MT-B-01**` …): ersetze `[ ]` durch `[x]` (PASS), `[!]` (FAIL), oder `[-]` (SKIP) UND füge eine eingerückte Zeile darunter:
     ```markdown
       _Befund (2026-06-25, Tester: <username>): <PASS|FAIL <grund>|SKIP <grund>>_
     ```
   - **Wenn dieser Testfall schon mal beim Befundet wurde** (vorheriger Lauf): ergänze, **überschreibe nicht**. Datum/Tester-Vermerk macht historisch nachvollziehbar.
   - Username heuristisch aus `git config user.name` oder Umgebung — wenn unklar: einmalig zu Beginn fragen ("Wer testet heute? (für Befund-Vermerk)").

5. **Zwischen-Statistik alle 5 Testfälle**:
   ```
   📊 Zwischenstand (5 abgehakt von 17):  ✓ 3   ✗ 1   ⊝ 1
   Weiter mit MT-B-06? [ja | pause | quit]
   ```

6. **Am Ende — Abschlussbericht**:
   ```
   ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
   ✅ Lauf abgeschlossen: <spec-name>
   ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

   Gesamt: 17  ✓ 12 PASS   ✗ 3 FAIL   ⊝ 2 SKIP
   Dauer: ~<HH:MM>

   🔴 Failures:
   - MT-B-04: <Grund>
   - MT-B-09: <Grund>
   - MT-K-02: <Grund>

   ⊝ Skips:
   - MT-S-03: <Grund>
   - …

   Datei aktualisiert: specs/013-customer-portal/checklists/manual-test.md
   ```

7. **Folgeaktion** anbieten:
   ```
   Soll ich:
   - die FAIL-Befunde als GitHub-Issues erfassen (`/speckit.taskstoissues` o.ä.)?
   - ein Bugfix-Branch + Commit-Stub vorbereiten?
   - den Run committen (`docs(test): testlauf <spec> vom 2026-06-25`)?
   - nichts (nur abschließen)?
   ```

## Leitlinien

- **Schreibe sofort, nicht batched** — wenn der Tester zwischendrin Strom verliert, dürfen keine Befunde verloren gehen.
- **Format-Treue**: ändere nur Befund-Zeilen + Checkbox-Zustand, **nichts anderes** am Drehbuch (kein "ich verbessere mal eben den Titel").
- **Falls die Datei git-dirty ist** vor Start: einmal warnen, dann fortfahren (Berater soll selbst stagen).
- **Kein Auto-Click** im UI — du bist Co-Pilot, nicht Auto-Pilot. Der Mensch macht die Aktion, du dokumentierst.
- **Bei "back"**: setze die vorher geschriebene Befund-Zeile zurück (entferne sie), markiere Checkbox wieder als `[ ]`.
- Wenn `--dry-run`: zeige die Reihenfolge und Anzahl, schreibe NICHTS in Datei, beende.
- **Don't be cute**: keine Emojis im commit, keine extra Prosa zwischen den Testfällen. Tester will durchkommen.
