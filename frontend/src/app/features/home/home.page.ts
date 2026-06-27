import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

import { HomepageAreasService } from '../../core/homepage/homepage-areas.service';
import { HomepageAreaTile } from '../../shared/homepage-area-state';

type HomePageResponseState = {
  page: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
  isEmpty: boolean;
};

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <section class="homepage-shell">
      <header class="headline">
        <p class="eyebrow">Startseite</p>
        <h1>Kundenbereiche</h1>
        <p class="lede">Finde schnell den passenden Bereich und oeffne ihn direkt aus der Startseite.</p>
      </header>

      <section class="controls" aria-label="Kundenbereiche filtern">
        <label>
          Suche
          <input
            [value]="searchTerm()"
            placeholder="Bereich oder Kunde suchen"
            (input)="setSearch($any($event.target).value)"
          />
        </label>
        <button type="button" (click)="load(1)">Aktualisieren</button>
      </section>

      <p class="error" *ngIf="errorMessage()">{{ errorMessage() }}</p>

      <section *ngIf="!response()?.isEmpty; else emptyState" class="tile-grid">
        <article class="tile" *ngFor="let area of areas()">
          <h2>{{ area.areaName }}</h2>
          <p>Bereichsstatus: {{ area.areaStatus }}</p>
          <a [routerLink]="area.navigationTarget">Bereich oeffnen</a>
        </article>
      </section>

      <ng-template #emptyState>
        <section class="empty-state">
          <h2>Noch keine Kundenbereiche verfuegbar</h2>
          <p>
            Sobald dir Bereiche zugewiesen sind, erscheinen sie hier als Kacheln.
          </p>
        </section>
      </ng-template>

      <footer class="pagination" *ngIf="response() as data">
        <button type="button" (click)="load(data.page - 1)" [disabled]="data.page <= 1">
          Zurueck
        </button>
        <span>Seite {{ data.page }} / {{ data.totalPages || 1 }}</span>
        <button type="button" (click)="load(data.page + 1)" [disabled]="data.totalPages === 0 || data.page >= data.totalPages">
          Weiter
        </button>
      </footer>
    </section>
  `,
  styles: [`
    .homepage-shell {
      margin-top: 24px;
      display: grid;
      gap: 20px;
    }

    .headline {
      background: linear-gradient(135deg, var(--color-primary) 0%, #5f2f75 100%);
      color: white;
      border-radius: 20px;
      padding: 24px;
    }

    .eyebrow {
      margin: 0;
      text-transform: uppercase;
      letter-spacing: 0.08em;
      font-size: 0.78rem;
      opacity: 0.85;
    }

    h1 {
      margin: 8px 0 10px;
      font-size: clamp(1.8rem, 3vw, 2.6rem);
    }

    .lede {
      margin: 0;
      max-width: 52ch;
      color: rgba(255, 255, 255, 0.92);
    }

    .controls {
      display: flex;
      flex-wrap: wrap;
      gap: 12px;
      align-items: end;
    }

    label {
      display: grid;
      gap: 6px;
      font-weight: 600;
      min-width: 260px;
    }

    input {
      border: 1px solid var(--color-border);
      border-radius: 10px;
      padding: 10px 12px;
      font: inherit;
    }

    button {
      border: 0;
      border-radius: 10px;
      padding: 10px 14px;
      background: var(--color-secondary);
      color: #083133;
      font-weight: 700;
      cursor: pointer;
    }

    button:disabled {
      opacity: 0.5;
      cursor: not-allowed;
    }

    .tile-grid {
      display: grid;
      gap: 14px;
      grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
    }

    .tile {
      background: var(--color-surface-soft);
      border: 1px solid var(--color-border);
      border-radius: 14px;
      padding: 16px;
      display: grid;
      gap: 8px;
    }

    .tile h2 {
      margin: 0;
      font-size: 1.05rem;
    }

    .tile p {
      margin: 0;
      color: #4c5560;
    }

    .tile a {
      color: var(--color-primary);
      font-weight: 700;
      text-decoration: none;
    }

    .empty-state {
      border: 1px dashed var(--color-border);
      border-radius: 16px;
      padding: 22px;
      background: var(--color-primary-soft);
    }

    .empty-state h2 {
      margin: 0 0 8px;
    }

    .empty-state p {
      margin: 0;
    }

    .pagination {
      display: flex;
      gap: 10px;
      align-items: center;
    }

    .error {
      margin: 0;
      color: #a12622;
      font-weight: 600;
    }
  `],
})
export class HomePageComponent implements OnInit {
  private readonly homepageAreasService = inject(HomepageAreasService);
  private static readonly DefaultPageSize = 12;

  protected readonly searchTerm = signal('');
  protected readonly areas = signal<HomepageAreaTile[]>([]);
  protected readonly errorMessage = signal('');
  protected readonly response = signal<HomePageResponseState | null>(null);

  ngOnInit(): void {
    this.load(1);
  }

  protected setSearch(value: string): void {
    this.searchTerm.set(value);
    this.load(1);
  }

  protected load(page: number): void {
    this.homepageAreasService.listAreas({
      search: this.searchTerm(),
      page,
      pageSize: HomePageComponent.DefaultPageSize,
    }).subscribe({
      next: (result) => {
        this.areas.set(result.items);
        this.response.set(this.toResponseState(result));
        this.errorMessage.set('');
      },
      error: () => {
        this.errorMessage.set('Kundenbereiche konnten nicht geladen werden.');
        this.areas.set([]);
        this.response.set(this.createEmptyState());
      },
    });
  }

  private toResponseState(result: HomePageResponseState): HomePageResponseState {
    return {
      page: result.page,
      pageSize: result.pageSize,
      totalItems: result.totalItems,
      totalPages: result.totalPages,
      isEmpty: result.isEmpty,
    };
  }

  private createEmptyState(): HomePageResponseState {
    return {
      page: 1,
      pageSize: HomePageComponent.DefaultPageSize,
      totalItems: 0,
      totalPages: 0,
      isEmpty: true,
    };
  }
}
