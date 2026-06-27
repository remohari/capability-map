import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit, inject, signal } from '@angular/core';

import { AuthStateService } from '../../core/auth/auth-state.service';

@Component({
  selector: 'app-advisor-home-page',
  standalone: true,
  imports: [CommonModule],
  template: `
    <section class="feature-card">
      <p class="eyebrow">Berater:in</p>
      <h1>Beratungsfaelle</h1>
      <button type="button" (click)="loadCases()">Faelle laden</button>
      <pre *ngIf="cases() as value">{{ value | json }}</pre>
      <p class="error" *ngIf="error()">{{ error() }}</p>
    </section>
  `,
  styles: [`
    .feature-card { padding: 28px; border-radius: 24px; background: #ffffff; box-shadow: 0 16px 32px rgba(21,46,84,0.08); }
    .eyebrow { text-transform: uppercase; letter-spacing: 0.08em; color: #0b6d4f; font-size: 0.8rem; }
    button { border: 0; border-radius: 999px; padding: 12px 16px; font: inherit; cursor: pointer; color: white; background: #15876b; }
    pre { background: #f4f8fc; padding: 16px; border-radius: 16px; overflow: auto; }
    .error { color: #a12622; }
  `],
})
export class AdvisorHomePageComponent implements OnInit {
  private readonly http = inject(HttpClient);
  private readonly authState = inject(AuthStateService);

  protected readonly cases = signal<unknown | null>(null);
  protected readonly error = signal('');

  ngOnInit(): void {
    this.loadCases();
  }

  protected loadCases(): void {
    this.http.get('http://localhost:8080/api/advisor/cases', {
      headers: new HttpHeaders(this.authState.toRequestHeaders()),
    }).subscribe({
      next: (value) => {
        this.cases.set(value);
        this.error.set('');
      },
      error: (error) => {
        this.error.set(error?.error?.reasonCode ?? 'Faelle konnten nicht geladen werden.');
      },
    });
  }
}
