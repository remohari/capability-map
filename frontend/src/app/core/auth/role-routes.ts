export type BusinessRole = 'Kund:in' | 'Berater:in' | 'Bewerter:in' | 'Admin';
export const roleOptions: BusinessRole[] = ['Kund:in', 'Berater:in', 'Bewerter:in', 'Admin'];

export type ProtectedRoute = {
  path: string;
  allowedRoles: BusinessRole[];
  navigationLabel?: string;
};

// Placeholder route catalog for the authorization feature setup.
// Guard behavior and UI enforcement are added in later tasks.
export const protectedRoutes: ProtectedRoute[] = [
  {
    path: '/customer-areas',
    allowedRoles: ['Kund:in', 'Berater:in', 'Bewerter:in', 'Admin'],
    navigationLabel: 'Kundenbereiche',
  },
  {
    path: '/customer-areas/:areaId',
    allowedRoles: ['Kund:in', 'Berater:in', 'Bewerter:in', 'Admin'],
    navigationLabel: 'Bereichsdetails',
  },
  {
    path: '/customer',
    allowedRoles: ['Kund:in'],
    navigationLabel: 'Meine Inhalte',
  },
  {
    path: '/advisor',
    allowedRoles: ['Berater:in'],
    navigationLabel: 'Beratung',
  },
  {
    path: '/evaluator',
    allowedRoles: ['Bewerter:in'],
    navigationLabel: 'Bewertung',
  },
  {
    path: '/admin/roles',
    allowedRoles: ['Admin'],
    navigationLabel: 'Rollenverwaltung',
  },
];
