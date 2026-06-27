import { computed, inject, Injectable } from '@angular/core';

import { AuthStateService } from '../auth/auth-state.service';
import { evaluateRouteAccess } from '../auth/role-state';
import { protectedRoutes } from '../auth/role-routes';

@Injectable({ providedIn: 'root' })
export class NavigationConfigService {
  private readonly authState = inject(AuthStateService);

  readonly visibleRoutes = computed(() =>
    protectedRoutes.filter((route) =>
      evaluateRouteAccess(this.authState.currentUser(), route).state === 'allowed'),
  );
}
