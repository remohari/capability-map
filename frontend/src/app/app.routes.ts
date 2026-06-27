import { Routes } from '@angular/router';

import { adminAccessGuard } from './core/auth/admin-access.guard';
import { roleAccessGuard } from './core/auth/role-access.guard';
import { RoleManagementPageComponent } from './features/admin/role-management/role-management.page';
import { AdvisorHomePageComponent } from './features/advisor/advisor-home.page';
import { CustomerHomePageComponent } from './features/customer/customer-home.page';
import { EvaluatorHomePageComponent } from './features/evaluator/evaluator-home.page';
import { AccessDeniedPageComponent } from './shared/access-denied/access-denied.page';

export const appRoutes: Routes = [
  {
    path: '',
    redirectTo: 'customer',
    pathMatch: 'full',
  },
  {
    path: 'customer',
    canActivate: [roleAccessGuard],
    component: CustomerHomePageComponent,
  },
  {
    path: 'advisor',
    canActivate: [roleAccessGuard],
    component: AdvisorHomePageComponent,
  },
  {
    path: 'evaluator',
    canActivate: [roleAccessGuard],
    component: EvaluatorHomePageComponent,
  },
  {
    path: 'admin/roles',
    canActivate: [adminAccessGuard],
    component: RoleManagementPageComponent,
  },
  {
    path: 'access-denied',
    component: AccessDeniedPageComponent,
  },
];
