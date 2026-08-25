import { Injectable, signal } from '@angular/core';
import { HttpClient, httpResource } from '@angular/common/http';
import { PagedResult } from '../../../core/models/paged-result.model';
import { ApiResult } from '../../../core/models/api-result.model';
import { AddInvoiceItem, Invoice } from '../models/invoice.model';
import { environment } from '../../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class InvoiceService {
  private readonly baseUrl = `${environment.apiUrl}/invoices`;

  constructor(private http: HttpClient) {}

  page = signal(1);
  pageSize = signal(10);

  invoices = httpResource<PagedResult<Invoice>>(
    () => `${this.baseUrl}?page=${this.page()}&pageSize=${this.pageSize()}`,
    {
      defaultValue: { items: [], page: 1, pageSize: 10, totalItems: 0, totalPages: 0, message: '' }
    }
  );

  create() {
    return this.http.post<ApiResult<Invoice>>(this.baseUrl, {});
  }

  addItem(id: string, dto: AddInvoiceItem) {
    return this.http.post<ApiResult<Invoice>>(`${this.baseUrl}/${id}/items`, dto);
  }

  removeItem(id: string, itemId: string) {
    return this.http.delete<ApiResult<Invoice>>(`${this.baseUrl}/${id}/items/${itemId}`);
  }

  close(id: string) {
    return this.http.patch<ApiResult<Invoice>>(`${this.baseUrl}/${id}/close`, {});
  }

  delete(id: string) {
    return this.http.delete<ApiResult<null>>(`${this.baseUrl}/${id}`);
  }

  reload() {
    this.invoices.reload();
  }
}