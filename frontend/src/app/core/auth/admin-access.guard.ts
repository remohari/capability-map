import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

import { AuthStateService } from './auth-state.service';

export const adminAccessGuard: CanActivateFn = () => {
  const authState = inject(AuthStateService);
  const router = inject(Router);

  if (authState.currentUser().activeRole === 'Admin') {
    return true;
  }

  return router.createUrlTree(['/access-denied'], {
    queryParams: { reason: 'admin_only' },
  });
};
