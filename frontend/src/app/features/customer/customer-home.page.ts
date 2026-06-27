import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit, inject, signal } from '@angular/core';

import { AuthStateService } from '../../core/auth/auth-state.service';

@Component({
  selector: 'app-customer-home-page',
  standalone: true,
  imports: [CommonModule],
  template: `
    <section class="feature-card">
      <p class="eyebrow">Kund:in</p>
      <h1>Eigene Inhalte</h1>
      <p>Die Daten werden fuer den aktuell gesetzten Customer Scope geladen.</p>
      <button type="button" (click)="loadContent()">Eigene Inhalte laden</button>
      <pre *ngIf="content() as value">{{ value | json }}</pre>
      <p class="error" *ngIf="error()">{{ error() }}</p>
    </section>
  `,
  styles: [`
    .feature-card { padding: 28px; border-radius: 24px; background: #ffffff; box-shadow: 0 16px 32px rgba(21,46,84,0.08); }
    .eyebrow { text-transform: uppercase; letter-spacing: 0.08em; color: #1675d1; font-size: 0.8rem; }
    button { border: 0; border-radius: 999px; padding: 12px 16px; font: inherit; cursor: pointer; color: white; background: #1675d1; }
    pre { background: #f4f8fc; padding: 16px; border-radius: 16px; overflow: auto; }
    .error { color: #a12622; }
  `],
})
export class CustomerHomePageComponent implements OnInit {
  private readonly http = inject(HttpClient);
  private readonly authState = inject(AuthStateService);

  protected readonly content = signal<unknown | null>(null);
  protected readonly error = signal('');

  ngOnInit(): void {
    this.loadContent();
  }

  protected loadContent(): void {
    const scope = this.authState.currentUser().customerScope ?? 'customer-001';
    this.http.get(`http://localhost:8080/api/customer/content/${scope}`, {
      headers: new HttpHeaders(this.authState.toRequestHeaders()),
    }).subscribe({
      next: (value) => {
        this.content.set(value);
        this.error.set('');
      },
      error: (error) => {
        this.error.set(error?.error?.reasonCode ?? 'Inhalte konnten nicht geladen werden.');
      },
    });
  }
}
