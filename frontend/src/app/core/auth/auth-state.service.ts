import { Injectable, computed, signal } from '@angular/core';

import type { BusinessRole } from './role-routes';
import type { CurrentUserRoleState } from './role-state';

@Injectable({ providedIn: 'root' })
export class AuthStateService {
  private readonly currentUserSignal = signal<CurrentUserRoleState>({
    userId: 'admin-user',
    displayReference: 'admin@example.invalid',
    activeRole: 'Admin',
    status: 'active',
    customerScope: 'customer-001',
  });

  readonly currentUser = computed(() => this.currentUserSignal());

  setRole(role: BusinessRole): void {
    this.currentUserSignal.update((current) => ({
      ...current,
      activeRole: role,
      displayReference: `${role.toLowerCase().replace(/[^a-z]+/g, '-') || 'user'}@example.invalid`,
    }));
  }

  setCustomerScope(scope: string): void {
    this.currentUserSignal.update((current) => ({
      ...current,
      customerScope: scope,
    }));
  }

  toRequestHeaders(): Record<string, string> {
    const current = this.currentUser();
    return {
      'X-User-Id': current.userId,
      'X-Display-Reference': current.displayReference,
      'X-Active-Role': current.activeRole ?? '',
      'X-User-Status': current.status,
      'X-Customer-Scope': current.customerScope ?? '',
    };
  }
}
