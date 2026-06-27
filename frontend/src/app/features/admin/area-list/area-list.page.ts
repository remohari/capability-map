import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';

import type { AreaSummary } from '../../../core/auth/area-state';
import { AreaCreationService } from '../area-creation/area-creation.service';

@Component({
  selector: 'app-area-list-page',
  standalone: true,
  imports: [CommonModule],
  template: `
    <section class="page-shell">
      <h1>Bereiche</h1>
      <button type="button" (click)="load()">Aktualisieren</button>
      <p *ngIf="message()">{{ message() }}</p>

      <div *ngIf="areas().length === 0">Keine Bereiche vorhanden.</div>
      <article class="card" *ngFor="let area of areas()">
        <h2>{{ area.name }}</h2>
        <p>{{ area.customerName }}</p>
        <small>{{ area.status }}</small>
      </article>
    </section>
  `,
})
export class AreaListPageComponent implements OnInit {
  private readonly service = inject(AreaCreationService);
  protected readonly areas = signal<AreaSummary[]>([]);
  protected readonly message = signal('');

  ngOnInit(): void {
    this.load();
  }

  protected load(): void {
    this.service.listAreas().subscribe({
      next: (areas) => this.areas.set(areas),
      error: () => this.message.set('Bereiche konnten nicht geladen werden.'),
    });
  }
}
