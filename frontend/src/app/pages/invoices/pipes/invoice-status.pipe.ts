import { Pipe, PipeTransform } from '@angular/core';

const STATUS_LABELS: Record<string, string> = {
  Open: 'Aberta',
  Closed: 'Fechada'
};

@Pipe({
  name: 'invoiceStatus',
  standalone: true
})
export class InvoiceStatusPipe implements PipeTransform {
  transform(status: string): string {
    return STATUS_LABELS[status] ?? status;
  }
}