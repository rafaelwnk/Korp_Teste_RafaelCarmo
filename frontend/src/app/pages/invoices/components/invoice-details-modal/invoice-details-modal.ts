import { Component, input } from '@angular/core';
import { Invoice } from '../../models/invoice.model';
import { Modal } from '../../../../shared/components/modal/modal';
import { DatePipe } from '@angular/common';
import { InvoiceStatusPipe } from "../../pipes/invoice-status.pipe";

@Component({
  imports: [Modal, DatePipe, InvoiceStatusPipe],
  selector: 'app-invoice-details-modal',
  templateUrl: './invoice-details-modal.html',
})
export class InvoiceDetailsModal {
  invoice = input<Invoice | undefined>(undefined);
}
