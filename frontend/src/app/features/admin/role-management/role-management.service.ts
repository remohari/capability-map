import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';

import { Observable } from 'rxjs';

import { AuthStateService } from '../../../core/auth/auth-state.service';
import type { BusinessRole } from '../../../core/auth/role-routes';

export type RoleAssignmentRecord = {
  assignmentId: string;
  userId: string;
  roleName: BusinessRole;
  assignedBy: string;
  assignedAt: string;
  changeReason?: string | null;
};

@Injectable({ providedIn: 'root' })
export class RoleManagementService {
  private readonly http = inject(HttpClient);
  private readonly authState = inject(AuthStateService);
  private readonly apiBaseUrl = 'http://localhost:8080';

  getAssignments(): Observable<RoleAssignmentRecord[]> {
    return this.http.get<RoleAssignmentRecord[]>(`${this.apiBaseUrl}/api/role-assignments`);
  }

  assignRole(userId: string, roleName: BusinessRole, changeReason: string): Observable<RoleAssignmentRecord> {
    return this.http.put<RoleAssignmentRecord>(
      `${this.apiBaseUrl}/api/role-assignments/${userId}`,
      { roleName, changeReason },
      { headers: this.createHeaders() },
    );
  }

  removeRole(userId: string): Observable<void> {
    return this.http.delete<void>(
      `${this.apiBaseUrl}/api/role-assignments/${userId}`,
      { headers: this.createHeaders() },
    );
  }

  private createHeaders(): HttpHeaders {
    return new HttpHeaders(this.authState.toRequestHeaders());
  }
}
