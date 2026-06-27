import type { BusinessRole, ProtectedRoute } from './role-routes';

export type RoleAccessState = 'allowed' | 'denied' | 'missing-role' | 'inactive';

export type CurrentUserRoleState = {
  userId: string;
  displayReference: string;
  activeRole: BusinessRole | null;
  status: 'active' | 'inactive' | 'suspended';
  customerScope?: string | null;
};

export type RouteAccessDecision = {
  route: ProtectedRoute;
  state: RoleAccessState;
  reasonCode: string;
};

export function evaluateRouteAccess(
  user: CurrentUserRoleState,
  route: ProtectedRoute,
): RouteAccessDecision {
  if (user.status !== 'active') {
    return {
      route,
      state: 'inactive',
      reasonCode: 'inactive_user',
    };
  }

  if (!user.activeRole) {
    return {
      route,
      state: 'missing-role',
      reasonCode: 'missing_role',
    };
  }

  if (!route.allowedRoles.includes(user.activeRole)) {
    return {
      route,
      state: 'denied',
      reasonCode: 'wrong_role',
    };
  }

  return {
    route,
    state: 'allowed',
    reasonCode: 'allowed',
  };
}
