export type HomepageAreaTile = {
  areaId: string;
  customerId: string;
  areaName: string;
  areaStatus: string;
  navigationTarget: string;
};

export type HomepageAreasResponse = {
  items: HomepageAreaTile[];
  page: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
  isEmpty: boolean;
};

export type HomepageAreasQuery = {
  search?: string;
  page?: number;
  pageSize?: number;
};
