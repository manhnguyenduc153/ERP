export type Warehouse = {
  warehouseId: number;
  warehouseName: string;
  location: string;
};

export type ApiResponse<T> = {
  data: T | null;
  metaData: any | null;
  message: string;
  success: boolean;
  statusCode: number;
};
