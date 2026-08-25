export interface ApiResult<T> {
  data: T | null;
  message: string;
}