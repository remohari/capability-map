import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AreaCreationService } from './area-creation.service';

@Component({
  selector: 'app-area-creation-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <section class="page-shell">
      <h1>Bereich anlegen</h1>
      <form (ngSubmit)="submit()" class="card">
        <label>
          Kundenname
          <input [(ngModel)]="customerName" name="customerName" required />
        </label>
        <label>
          Kunden-ID
          <input [(ngModel)]="customerIdentifier" name="customerIdentifier" required />
        </label>
        <label>
          Bereichsname
          <input [(ngModel)]="areaName" name="areaName" required />
        </label>
        <label>
          Beschreibung
          <textarea [(ngModel)]="areaDescription" name="areaDescription"></textarea>
        </label>
        <button type="submit">Bereich speichern</button>
        <p *ngIf="message()">{{ message() }}</p>
      </form>
    </section>
  `,
})
export class AreaCreationPageComponent {
  private readonly service = inject(AreaCreationService);

  protected customerName = '';
  protected customerIdentifier = '';
  protected areaName = '';
  protected areaDescription = '';
  protected readonly message = signal('');

  protected submit(): void {
    this.service
      .createArea({
        customerName: this.customerName,
        customerIdentifier: this.customerIdentifier,
        areaName: this.areaName,
        areaDescription: this.areaDescription,
      })
      .subscribe({
        next: (area) => {
          this.message.set(`Bereich ${area.name} wurde angelegt.`);
        },
        error: () => {
          this.message.set('Bereich konnte nicht angelegt werden.');
        },
      });
  }
}
