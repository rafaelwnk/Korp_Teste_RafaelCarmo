export interface PagedResult<T> {
  items: T[] | null;
  page: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
  message: string;
}