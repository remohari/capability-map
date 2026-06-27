import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

@Component({
  selector: 'app-access-denied-page',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <section class="denied-shell">
      <p class="eyebrow">Berechtigung</p>
      <h1>Zugriff verweigert</h1>
      <p>
        Der angeforderte Bereich ist fuer die aktuelle Rolle nicht freigegeben.
        Grund: <strong>{{ reason }}</strong>
      </p>
      <p *ngIf="target">Ziel: <code>{{ target }}</code></p>
      <a routerLink="/">Zurueck zur Startseite</a>
    </section>
  `,
  styles: [`
    .denied-shell {
      padding: 36px;
      border-radius: 24px;
      background: linear-gradient(145deg, #fff7f7, #fff0f0);
      border: 1px solid rgba(184, 56, 56, 0.16);
      box-shadow: 0 18px 42px rgba(114, 31, 31, 0.09);
    }
    .eyebrow { text-transform: uppercase; letter-spacing: 0.08em; color: #a12622; font-size: 0.8rem; }
    h1 { margin: 8px 0 12px; color: #7c1f1f; }
    a { color: #0d5aad; font-weight: 700; text-decoration: none; }
    code { font-family: Consolas, monospace; }
  `],
})
export class AccessDeniedPageComponent {
  private readonly route = inject(ActivatedRoute);

  protected readonly reason = this.route.snapshot.queryParamMap.get('reason') ?? 'unknown';
  protected readonly target = this.route.snapshot.queryParamMap.get('target');
}
