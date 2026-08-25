import { Injectable, signal } from '@angular/core';
import { HttpClient, httpResource } from '@angular/common/http';
import { PagedResult } from '../../../core/models/paged-result.model';
import { ApiResult } from '../../../core/models/api-result.model';
import { AdjustStock, CreateProduct, Product, UpdateDescription } from '../models/product.model';
import { environment } from '../../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private readonly baseUrl = `${environment.apiUrl}/products`;

  constructor(private http: HttpClient) {}

  page = signal(1);
  pageSize = signal(10);

  products = httpResource<PagedResult<Product>>(
    () => `${this.baseUrl}?page=${this.page()}&pageSize=${this.pageSize()}`,
    {
      defaultValue: { items: [], page: 1, pageSize: 10, totalItems: 0, totalPages: 0, message: '' }
    }
  );

  productId = signal<string | undefined>(undefined);

  product = httpResource<ApiResult<Product>>(() => {
    const id = this.productId();
    return id ? `${this.baseUrl}/${id}` : undefined;
  });

  create(dto: CreateProduct) {
    return this.http.post<ApiResult<Product>>(this.baseUrl, dto);
  }

  increaseStock(id: string, dto: AdjustStock) {
    return this.http.patch<ApiResult<Product>>(`${this.baseUrl}/${id}/increase`, dto);
  }

  decreaseStock(id: string, dto: AdjustStock) {
    return this.http.patch<ApiResult<Product>>(`${this.baseUrl}/${id}/decrease`, dto);
  }

  updateDescription(id: string, dto: UpdateDescription) {
    return this.http.patch<ApiResult<Product>>(`${this.baseUrl}/${id}/description`, dto);
  }

  delete(id: string) {
    return this.http.delete<ApiResult<null>>(`${this.baseUrl}/${id}`);
  }

  reload() {
    this.products.reload();
  }
}