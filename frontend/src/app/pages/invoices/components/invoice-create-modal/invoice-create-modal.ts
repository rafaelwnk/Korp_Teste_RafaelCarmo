import { Component, output } from '@angular/core';
import { Invoice } from '../../models/invoice.model';
import { InvoiceService } from '../../services/invoice.service';
import { Modal } from '../../../../shared/components/modal/modal';

@Component({
  imports: [Modal],
  selector: 'app-invoice-create-modal',
  templateUrl: './invoice-create-modal.html',
})
export class InvoiceCreateModal {
  created = output<Invoice>();

  constructor(private invoiceService: InvoiceService) { }

  create() {
    this.invoiceService.create().subscribe(result => {
      if (result.data) {
        this.created.emit(result.data);
      }
    });
  }
}
