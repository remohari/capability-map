import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { AuthStateService } from '../../../core/auth/auth-state.service';
import type { AreaSummary } from '../../../core/auth/area-state';

export type CreateAreaRequest = {
  customerName: string;
  customerIdentifier: string;
  areaName: string;
  areaDescription?: string;
};

export type ManageAreaPermissionRequest = {
  userId: string;
  action: 'grant' | 'revoke';
  changeReason?: string;
};

@Injectable({ providedIn: 'root' })
export class AreaCreationService {
  private readonly http = inject(HttpClient);
  private readonly authState = inject(AuthStateService);
  private readonly apiBaseUrl = '';

  createArea(request: CreateAreaRequest): Observable<AreaSummary> {
    return this.http.post<AreaSummary>(`${this.apiBaseUrl}/api/areas`, request, {
      headers: this.createHeaders(),
    });
  }

  listAreas(): Observable<AreaSummary[]> {
    return this.http.get<AreaSummary[]>(`${this.apiBaseUrl}/api/areas`, {
      headers: this.createHeaders(),
    });
  }

  managePermission(areaId: string, request: ManageAreaPermissionRequest): Observable<unknown> {
    return this.http.post(`${this.apiBaseUrl}/api/areas/${areaId}/permissions`, request, {
      headers: this.createHeaders(),
    });
  }

  private createHeaders(): HttpHeaders {
    return new HttpHeaders(this.authState.toRequestHeaders());
  }
}
