import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

@Component({
  selector: 'app-customer-area-detail-page',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <section class="detail-card">
      <p class="eyebrow">Kundenbereich</p>
      <h1 *ngIf="isValidAreaId; else invalidTarget">Bereich {{ areaId }}</h1>
      <ng-template #invalidTarget>
        <h1>Ungueltiger Bereich</h1>
      </ng-template>
      <p *ngIf="isValidAreaId; else invalidHint">Du befindest dich in einem ausgewaehlten Kundenbereich.</p>
      <ng-template #invalidHint>
        <p>Das Ziel ist nicht gueltig oder nicht mehr verfuegbar.</p>
      </ng-template>
      <a routerLink="/customer-areas">Zurueck zur Uebersicht</a>
    </section>
  `,
  styles: [`
    .detail-card {
      margin-top: 24px;
      padding: 24px;
      border-radius: 18px;
      border: 1px solid var(--color-border);
      background: #fff;
      box-shadow: 0 12px 24px rgba(26, 33, 48, 0.08);
    }

    .eyebrow {
      margin: 0;
      color: var(--color-secondary);
      font-size: 0.8rem;
      text-transform: uppercase;
      letter-spacing: 0.08em;
    }

    h1 {
      margin: 8px 0;
    }

    a {
      color: var(--color-primary);
      font-weight: 700;
      text-decoration: none;
    }
  `],
})
export class CustomerAreaDetailPageComponent {
  private readonly route = inject(ActivatedRoute);

  protected readonly areaId = this.route.snapshot.paramMap.get('areaId') ?? 'unknown';
  protected readonly isValidAreaId = /^[0-9a-fA-F-]{8,}$/.test(this.areaId);
}
