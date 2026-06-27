import { CommonModule } from '@angular/common';
import { Component, computed, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

import {
  evaluateRouteAccess,
} from './core/auth/role-state';
import { protectedRoutes, roleOptions } from './core/auth/role-routes';
import { AuthStateService } from './core/auth/auth-state.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  template: `
    <main class="shell">
      <section class="hero">
        <div>
          <p class="eyebrow">Capability Map</p>
          <h1>Rollenkonzept als lauffaehige Demo implementiert.</h1>
          <p class="lede">
            Wechsle die aktive Rolle, pruefe die freigegebenen Bereiche und teste
            die Admin-Rollenverwaltung gegen die .NET-API.
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

      <section class="panel">
        <div class="panel-header">
          <div>
            <h2>Freigegebene Navigation</h2>
            <p class="status">
              Active user: <strong>{{ authState.currentUser().displayReference }}</strong>
              <span>Role: {{ authState.currentUser().activeRole }}</span>
            </p>
          </div>
          <nav class="nav-chips">
            <a
              *ngFor="let decision of visibleDecisions()"
              [routerLink]="decision.route.path"
              routerLinkActive="active-link"
              class="nav-chip"
            >
              {{ decision.route.navigationLabel }}
            </a>
          </nav>
        </div>

        <div class="route-list">
          <article class="route-card" *ngFor="let decision of accessDecisions()">
            <h3>{{ decision.route.navigationLabel }}</h3>
            <p>{{ decision.route.path }}</p>
            <span class="badge" [attr.data-state]="decision.state">{{ decision.state }}</span>
            <small>{{ decision.reasonCode }}</small>
          </article>
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

    .hero {
      display: grid;
      gap: 24px;
      padding: 32px;
      border-radius: 24px;
      background: linear-gradient(135deg, #0a4c8b 0%, #1675d1 100%);
      color: #ffffff;
      box-shadow: 0 18px 48px rgba(10, 76, 139, 0.25);
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

    .panel {
      margin-top: 28px;
      padding: 28px;
      border-radius: 24px;
      background: rgba(255, 255, 255, 0.84);
      box-shadow: 0 16px 38px rgba(19, 41, 75, 0.08);
      backdrop-filter: blur(14px);
    }

    h2 {
      margin-top: 0;
      color: #0f2748;
    }

    .panel-header {
      display: grid;
      gap: 16px;
    }

    .status {
      display: flex;
      gap: 12px;
      flex-wrap: wrap;
      margin-bottom: 20px;
      color: #35516f;
    }

    .route-list {
      display: grid;
      gap: 16px;
      grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
    }

    .nav-chips {
      display: flex;
      gap: 12px;
      flex-wrap: wrap;
    }

    .nav-chip {
      text-decoration: none;
      color: #0d4d89;
      font-weight: 700;
      background: #edf5ff;
      border-radius: 999px;
      padding: 10px 14px;
    }

    .active-link {
      background: #0d4d89;
      color: white;
    }

    .route-card {
      padding: 18px;
      border-radius: 18px;
      background: #f5f9ff;
      border: 1px solid rgba(22, 117, 209, 0.12);
    }

    .route-card h3 {
      margin: 0 0 8px;
    }

    .route-card p {
      margin: 0 0 12px;
      color: #4b6582;
    }

    .badge {
      display: inline-flex;
      align-items: center;
      border-radius: 999px;
      padding: 6px 10px;
      font-size: 0.82rem;
      font-weight: 700;
      text-transform: uppercase;
      background: #dff4e4;
      color: #146c2e;
    }

    .badge[data-state="denied"],
    .badge[data-state="missing-role"],
    .badge[data-state="inactive"] {
      background: #ffe5e5;
      color: #a12622;
    }

    small {
      display: block;
      margin-top: 10px;
      color: #4b6582;
    }
  `],
})
export class AppComponent {
  protected readonly authState = inject(AuthStateService);
  protected readonly roles = roleOptions;
  protected readonly accessDecisions = computed(() =>
    protectedRoutes.map((route) => evaluateRouteAccess(this.authState.currentUser(), route)),
  );
  protected readonly visibleDecisions = computed(() =>
    this.accessDecisions().filter((decision) => decision.state === 'allowed'),
  );

  protected setRole(role: string): void {
    this.authState.setRole(role as typeof roleOptions[number]);
  }

  protected setCustomerScope(scope: string): void {
    this.authState.setCustomerScope(scope);
  }
}
