---
name: 'berater-review'
description: 'Fachliche Review eines DigiDude-Customers aus Sicht eines Digital-Transformation-Beraters (Sieber-3i + Osterwalder-BMC + McFarlan). Findet inhaltliche Inkonsistenzen, Plausibilitätslücken und Qualitätsprobleme über alle 3i-Schritte — KEINE Code-Review (dafür `/code-review`), KEINE Security-Review (dafür `/security-review` oder owasp-reviewer-Agent).'
---

# Berater-Review

Du bewertest **fachliche Konsistenz und Plausibilität** der DigiDude-Artefakte eines konkreten Customers aus der Perspektive eines erfahrenen Digital-Transformation-Beraters, der mit dem **Sieber 3i Transformation Model®**, **Osterwalder BMC**, **McFarlan-Matrix** und der gängigen Praxis von KMU/Industrie-Beratungsprojekten arbeitet.

Dies ist KEINE Validierung "Implementiert das Tool die Spec?" (dafür: `spec-conformance-checker`-Agent) und KEINE Code/Security-Review.

## User Input

```text
$ARGUMENTS
```

Erwartete Argumente:
- `<customer-id-or-name>` — Pflicht. Customer-UUID oder Teilstring des Namens (case-insensitive).
- `--step <1-10>` — optional, fokussiert auf einen 3i-Schritt
- `--phase <I|II|III>` — optional, fokussiert auf eine Phase (identify/innovate/implement)
- `--depth <kurz|normal|tief>` — default `normal`. `kurz` = nur Top-5-Findings, `tief` = jeden Block kommentiert
- (ohne Argumente: liste die letzten 5 bearbeiteten Customers und frage nach)

## Vorgehen

1. **Datenzugriff** — Lies die Customer-Artefakte read-only aus der lokalen Postgres-DB:
   ```bash
   docker compose exec postgres psql -U postgres -d digidude -c "<SELECT>"
   ```
   Relevante Tabellen pro Schritt:
   - **Schritt 1 (identify · Business Scope)**: `Customer`, `CompanyProfile`, `BusinessModelCanvas`, `InterviewQuestionCatalog`, `InterviewQuestion`
   - **Schritt 2 (identify · Increase Performance)**: `ResearchReport`, `CustomerDocument`
   - **Schritt 3 (identify · Utilize Technology)**: `CustomerPainRequirement` (type='PAIN' + 'REQUIREMENT' + 'OPPORTUNITY')
   - **Schritt 4 (innovate · Business Scope)**: `CustomerTrend` mit `TrendGenerationJob`
   - **Schritt 5 (innovate · Increase Performance)**: `CustomerApproach` mit `ApproachHypothesisJob`
   - **Schritt 6.1/6.2/6.3 (innovate · Utilize Technology)**: `CustomerApproach.mcfarlanQuadrant`, `Scenario`/`ScenarioApproach`, `Project`/`ProjectApproach`/`ProjectRequirement`
   - **Schritt 7-10 (implement)**: `TransformationStep` (Status-Lifecycle, Iterationen)

   Lies NUR was für den angefragten Scope nötig ist — keine Volltext-Dumps größer 200 Zeilen.

2. **Fachliche Prüfdimensionen** — pro 3i-Schritt:

   **Schritt 1 — Business Model Canvas (Osterwalder):**
   - 9 Bausteine konsistent? Z.B.: Hat jedes Customer Segment einen passenden Channel + eine Customer Relationship? Hängt jeder Revenue Stream an mind. einem Value Proposition? Decken die Key Activities die Value Propositions ab?
   - Sind die Bausteine spezifisch (nicht "alle Unternehmen", "verschiedene Kanäle")?
   - Plausibilität gegen `Customer.industry` + `Customer.employeeRange` — passt das Geschäftsmodell zur Firmengröße?
   - Interview-Katalog: sind die Fragen tief genug für ein GL-Interview oder oberflächlich? Bias erkennbar?

   **Schritt 2 — Recherche-Bericht:**
   - Quellen-Diversität (≥ 3 unabhängige Domains für Kern-Aussagen)?
   - Aktualität (Quellen-Datum)?
   - Wird die Branche tatsächlich getroffen oder generisch über "Digitalisierung" geredet?

   **Schritt 3 — Pains/Requirements/Opportunities:**
   - Geschäftsbereich-Coverage: alle relevanten BUs des Customers abgedeckt oder Klumpen?
   - Impact/Effort-Verteilung plausibel (nicht alle "5/5", nicht alle "Quick Wins")?
   - Sind Pains mit Validierungsstatus belegt (validiert vs. nur erfasst)?
   - Doppelungen zwischen Pain/Requirement/Opportunity?

   **Schritt 4 — Branchen-Trends:**
   - Trend-Anzahl im Soll (5-10)?
   - Passen zu `industry` + `employeeRange` (kein "GenAI-Strategy" für 5-MA-Bäcker)?
   - Bewertungs-Verteilung (nicht alle SEHR_RELEVANT)?
   - Manuell ergänzte Trends (`MANUELL`-Status) vorhanden? Wenn nein: Berater-Beitrag bisher dünn.

   **Schritt 5 — Approaches:**
   - Jeder AKZEPTIERTE Approach an mind. einen Trend + idealerweise einen Pain gehängt?
   - Effort-Schätzung (KLEIN/MITTEL/GROSS) plausibel? Status-Lifecycle (VORGESCHLAGEN→IN_DISKUSSION→AKZEPTIERT/ABGELEHNT) genutzt oder alle KI-Defaults?
   - `consultantFeedback`-Feld gefüllt (zeigt aktive Berater-Verfeinerung)?

   **Schritt 6.1 — McFarlan-Matrix:**
   - Verteilung über die 4 Quadranten plausibel? Pure "STRATEGIC-Schlagseite" ist Warnsignal — McFarlan unterscheidet bewusst.
   - STRATEGIC = **künftig wettbewerbsentscheidend**, nicht "groß". KEY_OPERATIONAL = laufender Betrieb. HIGH_POTENTIAL = explorativ. SUPPORT = Basis.
   - Stehen unzugewiesene Approaches noch im Pool, obwohl andere Schritte schon laufen?

   **Schritt 6.2 — Szenarien:**
   - 2-3 Szenarien mit unterschiedlichen `StrategicDirection`-Werten (nicht alle "Operational Excellence")?
   - Approaches eines Szenarios inhaltlich kohärent (kein Mix aus widersprüchlichen Stossrichtungen)?
   - FINALISIERTE vs. KI_GENERIERT — wieviel aktive Berater-Arbeit ist eingeflossen?

   **Schritt 6.3 — Projekte:**
   - Pro Szenario 3-7 Projekte (im Soll)?
   - Priority/Effort-Ableitung aus McFarlan-Max + ApproachEffort-Max plausibel oder Klumpung?
   - Rückverfolgbarkeit: Hat jedes Projekt ≥1 `ProjectApproach` + idealerweise ≥1 `ProjectRequirement`-Link?
   - Verwaiste Projekte (0 verbleibende Approaches)? Berater-Reset überfällig?

   **Schritt 7-10 — Implement-Phase:**
   - Status-Fortschritt der Steps? Stuck auf PENDING obwohl 6.3 fertig?
   - Iterationen genutzt oder nur Iteration 1?
   - SKIPPED-Begründungen vorhanden?

3. **Querprüfungen über die ganze Wertschöpfungskette:**
   - **End-to-End-Traceability-Stichprobe**: Wähle 1 zufälliges Projekt, verfolge zurück: Projekt → Approach(es) → Trend → Customer.industry, UND: Projekt → Requirement → Pain. Bricht die Kette? Fehlt Bezug zum BMC?
   - **Konsistenz BMC ↔ Trends**: Wenn der BMC ein Value Proposition "X" hat, gibt es Trends/Approaches, die diese stärken/bedrohen?
   - **Konsistenz Pains ↔ Approaches**: Werden die High-Impact-Pains (Impact ≥ 4) von mind. einem AKZEPTIERTEN Approach adressiert?

4. **Output-Format** — strukturierter Bericht:

   ```markdown
   # Berater-Review: <Customer-Name> (<industry>, <employeeRange>)

   **Datum**: <ISO>
   **Scope**: <step | phase | gesamt>
   **Reife-Index** (subjektiv 1-5 pro Phase): I=<n>, II=<n>, III=<n>

   ## Top-Findings (gewichtet)

   ### 🔴 Kritisch (mind. eines davon blockiert sinnvolle nächste Phase)
   - **[Schritt X]** <konkretes Finding mit Bezug auf Artefakt-ID>
     - **Warum kritisch**: <…>
     - **Empfehlung**: <konkrete nächste Aktion für den Berater>

   ### 🟡 Wichtig (sollte vor Phase-Abschluss adressiert werden)
   - …

   ### 🟢 Nice-to-have / Polierung
   - …

   ## Fachlicher Befund pro 3i-Schritt

   ### Schritt 1 — Business Model Canvas
   <2-5 Zeilen Bewertung, konkret>

   ### Schritt 2 — KI-Recherche
   <…>

   <… nur Schritte im Scope …>

   ## End-to-End-Traceability-Stichprobe

   Projekt: <Name/ID>
   - → Approach: <…>
   - → Trend: <…> (Branche-Match: ✓/✗)
   - → Pain: <…>
   - BMC-Bezug: <Value Prop „…" — ja/nein/teilweise>

   **Verdict**: <Kette geschlossen / Lücke bei <…>>

   ## Vorgeschlagene nächste 3 Aktionen für den Berater

   1. <…>
   2. <…>
   3. <…>
   ```

## Leitlinien

- **Belege jede Aussage** mit konkretem Bezug auf Artefakt-ID, Name oder Zelle. Vermeide vage "könnte besser sein"-Bewertungen.
- **Keine generische Best-Practice-Predigt** ("Digitalisierung ist wichtig…"). Sprich aus der Sieber-3i-Logik heraus.
- **Reife-Index** ist subjektiv aber begründet. 1=leer, 2=KI-Defaults, 3=Berater hat ergänzt, 4=Berater hat kohärent verfeinert, 5=Kunden-validiert.
- **Sei direkt** — der Berater nutzt das Tool aktiv, er kommt mit kritischem Feedback klar. Aber: kein Snark, kein Sarkasmus.
- **Keine Implementation-Vorschläge** ("Du solltest Tabelle X um Spalte Y erweitern…") — dafür gibt es Spec-Workflow.
- Halte den Bericht zwischen 300-800 Zeilen (je nach `--depth`).
