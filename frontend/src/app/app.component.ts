import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

import { roleOptions } from './core/auth/role-routes';
import { AuthStateService } from './core/auth/auth-state.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  template: `
    <main class="shell">
      <header class="topbar">
        <a routerLink="/" class="brand" aria-label="Zur Startseite">
          <img src="/asserts/Nordmap.png" alt="Nordmap" />
          <span>Capability Map</span>
        </a>

        <nav class="main-nav" aria-label="Hauptnavigation">
          <a routerLink="/" routerLinkActive="active-link" [routerLinkActiveOptions]="{ exact: true }">Startseite</a>
          <a routerLink="/customer-areas" routerLinkActive="active-link">Kundenbereiche</a>
        </nav>
      </header>

      <section class="hero">
        <div>
          <p class="eyebrow">Rollen-Demo</p>
          <h1>Startseite mit Kundenbereich-Kacheln</h1>
          <p class="lede">
            Wechsle die aktive Rolle oder den Customer Scope und pruefe, welche Kundenbereiche auf der Startseite sichtbar sind.
          </p>
        </div>

        <div class="control-panel">
          <label>
            Aktive Rolle
            <select [value]="authState.currentUser().activeRole ?? ''" (change)="setRole($any($event.target).value)">
              <option *ngFor="let role of roles" [value]="role">{{ role }}</option>
            </select>
          </label>
          <label>
            Customer Scope
            <input
              [value]="authState.currentUser().customerScope ?? ''"
              (input)="setCustomerScope($any($event.target).value)"
            />
          </label>
        </div>
      </section>

      <router-outlet />
    </main>
  `,
  styles: [`
    .shell {
      max-width: 960px;
      margin: 0 auto;
      padding: 48px 24px 80px;
    }

    .topbar {
      display: flex;
      justify-content: space-between;
      align-items: center;
      gap: 20px;
      flex-wrap: wrap;
      background: #ffffff;
      border: 1px solid var(--color-border);
      border-radius: 18px;
      padding: 14px 18px;
      box-shadow: 0 10px 24px rgba(26, 33, 48, 0.08);
    }

    .brand {
      display: inline-flex;
      align-items: center;
      gap: 10px;
      text-decoration: none;
      color: var(--color-text);
      font-weight: 700;
    }

    .brand img {
      width: 42px;
      height: 42px;
      object-fit: contain;
      border-radius: 8px;
      background: #fff;
    }

    .main-nav {
      display: flex;
      gap: 10px;
      flex-wrap: wrap;
    }

    .main-nav a {
      text-decoration: none;
      color: var(--color-primary);
      border: 1px solid var(--color-border);
      background: var(--color-primary-soft);
      border-radius: 999px;
      padding: 9px 13px;
      font-weight: 700;
    }

    .main-nav a.active-link {
      background: var(--color-primary);
      color: #fff;
      border-color: var(--color-primary);
    }

    .hero {
      display: grid;
      gap: 24px;
      padding: 32px;
      border-radius: 24px;
      background: linear-gradient(135deg, var(--color-secondary) 0%, #288f90 100%);
      color: #ffffff;
      box-shadow: 0 18px 48px rgba(20, 104, 102, 0.25);
      margin-top: 20px;
    }

    .eyebrow {
      margin: 0 0 12px;
      text-transform: uppercase;
      letter-spacing: 0.08em;
      font-size: 0.78rem;
      opacity: 0.82;
    }

    h1 {
      margin: 0;
      font-size: clamp(2rem, 5vw, 3.4rem);
      line-height: 1.05;
    }

    .lede {
      max-width: 52rem;
      margin: 16px 0 0;
      font-size: 1.05rem;
      line-height: 1.6;
      color: rgba(255, 255, 255, 0.9);
    }

    .control-panel {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
      gap: 14px;
    }

    label {
      display: grid;
      gap: 8px;
      font-weight: 600;
    }

    select, input {
      border: 0;
      border-radius: 14px;
      padding: 12px 14px;
      font: inherit;
      background: rgba(255,255,255,0.94);
      color: #153357;
    }
  `],
})
export class AppComponent {
  protected readonly authState = inject(AuthStateService);
  protected readonly roles = roleOptions;

  protected setRole(role: string): void {
    this.authState.setRole(role as typeof roleOptions[number]);
  }

  protected setCustomerScope(scope: string): void {
    this.authState.setCustomerScope(scope);
  }
}
