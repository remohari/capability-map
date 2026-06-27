export type AreaAccessState = 'allowed' | 'denied' | 'no-permission' | 'wrong-scope';

export type AreaSummary = {
  areaId: string;
  customerId: string;
  customerName: string;
  name: string;
  description?: string | null;
  status: string;
};

export function canAccessArea(areaCustomerId: string, userScope?: string | null): AreaAccessState {
  if (!userScope) {
    return 'allowed';
  }

  if (areaCustomerId !== userScope) {
    return 'wrong-scope';
  }

  return 'allowed';
}
