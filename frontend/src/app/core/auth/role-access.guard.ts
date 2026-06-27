import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router } from '@angular/router';

import { AuthStateService } from './auth-state.service';
import { evaluateRouteAccess } from './role-state';
import { protectedRoutes } from './role-routes';

export const roleAccessGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const authState = inject(AuthStateService);
  const router = inject(Router);
  const path = `/${route.routeConfig?.path ?? ''}`.replace(/\/+/g, '/');
  const protectedRoute = protectedRoutes.find((entry) => entry.path === path);

  if (!protectedRoute) {
    return true;
  }

  const decision = evaluateRouteAccess(authState.currentUser(), protectedRoute);
  if (decision.state === 'allowed') {
    return true;
  }

  return router.createUrlTree(['/access-denied'], {
    queryParams: { reason: decision.reasonCode, target: path },
  });
};
