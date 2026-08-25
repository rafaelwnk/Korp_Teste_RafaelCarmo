import { Component, signal } from '@angular/core';
import { Invoice } from './models/invoice.model';
import { InvoiceService } from './services/invoice.service';
import { Pagination } from "../../shared/components/pagination/pagination";
import { InvoiceCreateModal } from "./components/invoice-create-modal/invoice-create-modal";
import { InvoiceDetailsModal } from "./components/invoice-details-modal/invoice-details-modal";
import { InvoiceEditModal } from "./components/invoice-edit-modal/invoice-edit-modal";
import { ConfirmModal } from "../../shared/components/confirm-modal/confirm-modal";
import { InvoiceStatusPipe } from "./pipes/invoice-status.pipe";

@Component({
  imports: [Pagination, InvoiceCreateModal, InvoiceDetailsModal, InvoiceEditModal, ConfirmModal, InvoiceStatusPipe],
  selector: 'app-invoices',
  templateUrl: './invoices.html',
})
export class Invoices {
  selectedInvoice = signal<Invoice | undefined>(undefined);

  constructor(protected invoiceService: InvoiceService) { }

  openDetails(invoice: Invoice) {
    this.selectedInvoice.set(invoice);
  }

  openEdit(invoice: Invoice) {
    this.selectedInvoice.set(invoice);
  }

  deleteInvoice() {
    const invoice = this.selectedInvoice();
    if (!invoice) return;

    this.invoiceService.delete(invoice.id).subscribe(() => {
      this.invoiceService.reload();
    });
  }
}
