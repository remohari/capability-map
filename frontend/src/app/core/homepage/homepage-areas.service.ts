import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { AuthStateService } from '../auth/auth-state.service';
import {
  HomepageAreasQuery,
  HomepageAreasResponse,
} from '../../shared/homepage-area-state';

@Injectable({ providedIn: 'root' })
export class HomepageAreasService {
  private readonly http = inject(HttpClient);
  private readonly authState = inject(AuthStateService);
  private readonly apiBaseUrl = '';

  listAreas(query: HomepageAreasQuery): Observable<HomepageAreasResponse> {
    let params = new HttpParams();
    if (query.search?.trim()) {
      params = params.set('search', query.search.trim());
    }

    if (query.page) {
      params = params.set('page', query.page);
    }

    if (query.pageSize) {
      params = params.set('pageSize', query.pageSize);
    }

    return this.http.get<HomepageAreasResponse>(`${this.apiBaseUrl}/api/home/customer-areas`, {
      headers: new HttpHeaders(this.authState.toRequestHeaders()),
      params,
    });
  }
}
