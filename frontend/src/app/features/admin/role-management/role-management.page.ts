import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AuthStateService } from '../../../core/auth/auth-state.service';
import { BusinessRole, roleOptions } from '../../../core/auth/role-routes';
import { RoleManagementService, type RoleAssignmentRecord } from './role-management.service';

@Component({
  selector: 'app-role-management-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <section class="page-shell">
      <header class="page-header">
        <div>
          <p class="eyebrow">Admin</p>
          <h1>Rollenverwaltung</h1>
          <p>Admins weisen genau eine aktive Rolle pro Nutzer:in zu und sehen sofort den aktuellen Stand.</p>
        </div>
        <div class="context-chip">
          Aktive Rolle: <strong>{{ authState.currentUser().activeRole }}</strong>
        </div>
      </header>

      <div class="admin-grid">
        <form class="card form-card" (ngSubmit)="submitAssignment()">
          <h2>Rolle zuweisen</h2>
          <label>
            Nutzer-ID
            <input [(ngModel)]="draftUserId" name="userId" required />
          </label>
          <label>
            Rolle
            <select [(ngModel)]="draftRole" name="role">
              <option *ngFor="let role of roles" [value]="role">{{ role }}</option>
            </select>
          </label>
          <label>
            Grund
            <textarea [(ngModel)]="draftReason" name="reason" rows="3"></textarea>
          </label>
          <button type="submit">Zuweisen oder aendern</button>
          <p class="message" *ngIf="message()">{{ message() }}</p>
        </form>

        <section class="card list-card">
          <div class="list-header">
            <h2>Aktuelle Rollen</h2>
            <button type="button" class="secondary" (click)="loadAssignments()">Aktualisieren</button>
          </div>
          <div class="empty-state" *ngIf="assignments().length === 0">Noch keine Rollenzuweisungen vorhanden.</div>
          <article class="assignment" *ngFor="let assignment of assignments()">
            <div>
              <h3>{{ assignment.userId }}</h3>
              <p>{{ assignment.roleName }} · {{ assignment.assignedBy }}</p>
            </div>
            <button type="button" class="secondary danger" (click)="removeAssignment(assignment.userId)">
              Rolle entfernen
            </button>
          </article>
        </section>
      </div>
    </section>
  `,
  styles: [`
    .page-shell { display: grid; gap: 24px; }
    .page-header, .admin-grid { display: grid; gap: 24px; }
    .admin-grid { grid-template-columns: repeat(auto-fit, minmax(280px, 1fr)); }
    .card {
      background: rgba(255,255,255,0.88);
      border: 1px solid rgba(15, 60, 117, 0.09);
      border-radius: 22px;
      padding: 24px;
      box-shadow: 0 18px 36px rgba(21, 46, 84, 0.08);
    }
    .eyebrow { text-transform: uppercase; letter-spacing: 0.08em; color: #0a5cb7; font-size: 0.8rem; margin: 0 0 10px; }
    h1, h2, h3 { margin: 0; }
    .context-chip {
      align-self: start;
      justify-self: start;
      padding: 10px 14px;
      background: #e6f2ff;
      border-radius: 999px;
      color: #0c4479;
      font-size: 0.95rem;
    }
    label { display: grid; gap: 8px; color: #24435e; font-weight: 600; }
    input, select, textarea {
      border-radius: 14px;
      border: 1px solid #bfd1e5;
      padding: 12px 14px;
      font: inherit;
      background: #fdfefe;
    }
    form { display: grid; gap: 16px; }
    button {
      border: 0;
      border-radius: 999px;
      padding: 12px 16px;
      font: inherit;
      font-weight: 700;
      cursor: pointer;
      color: #fff;
      background: linear-gradient(135deg, #0d4d89, #1675d1);
    }
    .secondary { background: #ebf3fb; color: #0d4374; }
    .danger { color: #7d1e1e; }
    .list-header { display: flex; justify-content: space-between; align-items: center; gap: 16px; margin-bottom: 16px; }
    .assignment {
      display: flex;
      justify-content: space-between;
      align-items: center;
      gap: 16px;
      padding: 14px 0;
      border-top: 1px solid rgba(15, 60, 117, 0.08);
    }
    .assignment:first-of-type { border-top: 0; }
    .assignment p, .message, .empty-state { color: #506882; }
  `],
})
export class RoleManagementPageComponent implements OnInit {
  protected readonly authState = inject(AuthStateService);
  private readonly roleManagementService = inject(RoleManagementService);

  protected readonly roles = roleOptions;
  protected readonly assignments = signal<RoleAssignmentRecord[]>([]);
  protected readonly message = signal('');

  protected draftUserId = 'customer-002';
  protected draftRole: BusinessRole = 'Kund:in';
  protected draftReason = 'Initiale Rollenzuweisung';

  ngOnInit(): void {
    this.loadAssignments();
  }

  protected loadAssignments(): void {
    this.roleManagementService.getAssignments().subscribe({
      next: (assignments) => this.assignments.set(assignments),
      error: () => this.message.set('Rollenzuweisungen konnten nicht geladen werden.'),
    });
  }

  protected submitAssignment(): void {
    this.roleManagementService.assignRole(this.draftUserId, this.draftRole, this.draftReason).subscribe({
      next: (assignment) => {
        this.message.set(`Rolle ${assignment.roleName} fuer ${assignment.userId} gespeichert.`);
        this.loadAssignments();
      },
      error: (error) => {
        this.message.set(error?.error?.reasonCode ?? 'Zuweisung fehlgeschlagen.');
      },
    });
  }

  protected removeAssignment(userId: string): void {
    this.roleManagementService.removeRole(userId).subscribe({
      next: () => {
        this.message.set(`Rolle fuer ${userId} entfernt.`);
        this.loadAssignments();
      },
      error: (error) => {
        this.message.set(error?.error?.reasonCode ?? 'Entfernen fehlgeschlagen.');
      },
    });
  }
}
