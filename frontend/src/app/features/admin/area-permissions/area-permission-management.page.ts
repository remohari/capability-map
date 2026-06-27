import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { AreaCreationService } from '../area-creation/area-creation.service';

@Component({
  selector: 'app-area-permission-management-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <section class="page-shell">
      <h1>Bereichsberechtigungen</h1>
      <form (ngSubmit)="submit()" class="card">
        <label>
          Nutzer-ID
          <input [(ngModel)]="userId" name="userId" required />
        </label>
        <label>
          Aktion
          <select [(ngModel)]="action" name="action">
            <option value="grant">Freigeben</option>
            <option value="revoke">Entziehen</option>
          </select>
        </label>
        <label>
          Grund
          <textarea [(ngModel)]="changeReason" name="changeReason"></textarea>
        </label>
        <button type="submit">Speichern</button>
        <p *ngIf="message()">{{ message() }}</p>
      </form>
    </section>
  `,
})
export class AreaPermissionManagementPageComponent {
  private readonly service = inject(AreaCreationService);
  private readonly route = inject(ActivatedRoute);

  protected userId = '';
  protected action: 'grant' | 'revoke' = 'grant';
  protected changeReason = '';
  protected readonly message = signal('');

  protected submit(): void {
    const areaId = this.route.snapshot.paramMap.get('areaId');
    if (!areaId) {
      this.message.set('Ungueltige Bereichs-ID.');
      return;
    }

    this.service
      .managePermission(areaId, {
        userId: this.userId,
        action: this.action,
        changeReason: this.changeReason,
      })
      .subscribe({
        next: () => this.message.set('Berechtigung gespeichert.'),
        error: () => this.message.set('Berechtigung konnte nicht gespeichert werden.'),
      });
  }
}
