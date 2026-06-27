import { Route } from '@angular/router';
import { adminAccessGuard } from '../../core/auth/admin-access.guard';

export const areaRoutes: Route[] = [
  {
    path: 'areas',
    loadComponent: () => import('./area-list/area-list.page').then(m => m.AreaListPageComponent),
    canMatch: [adminAccessGuard]
  },
  {
    path: 'areas/create',
    loadComponent: () => import('./area-creation/area-creation.page').then(m => m.AreaCreationPageComponent),
    canMatch: [adminAccessGuard]
  },
  {
    path: 'areas/:areaId/permissions',
    loadComponent: () => import('./area-permissions/area-permission-management.page').then(m => m.AreaPermissionManagementPageComponent),
    canMatch: [adminAccessGuard]
  },
  {
    path: 'areas/:areaId',
    loadComponent: () => import('./area-list/area-list.page').then(m => m.AreaListPageComponent),
    canMatch: [adminAccessGuard]
  },
  {
    path: 'customer-areas',
    loadComponent: () => import('./area-list/area-list.page').then(m => m.AreaListPageComponent),
    canMatch: [adminAccessGuard]
  }
];
