import { Component, input, output } from '@angular/core';
import { Invoice } from '../../models/invoice.model';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { InvoiceService } from '../../services/invoice.service';
import { ProductService } from '../../../products/services/product.service';
import { Modal } from "../../../../shared/components/modal/modal";
import { InvoiceStatusPipe } from "../../pipes/invoice-status.pipe";

@Component({
  imports: [Modal, ReactiveFormsModule, InvoiceStatusPipe],
  selector: 'app-invoice-edit-modal',
  templateUrl: './invoice-edit-modal.html',
})
export class InvoiceEditModal {
  invoice = input<Invoice | undefined>(undefined);
  saved = output<void>();

  form = new FormGroup({
    productId: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    quantity: new FormControl(1, { nonNullable: true, validators: [Validators.required, Validators.min(1)] })
  });

  constructor(
    private invoiceService: InvoiceService,
    protected productService: ProductService
  ) { }

  addItem() {
    const invoice = this.invoice();
    if (!invoice || this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const productId = this.form.value.productId!;
    const product = this.productService.products.value()?.items?.find(p => p.id === productId);
    if (!product) return;

    this.invoiceService
      .addItem(invoice.id, {
        productId,
        productCode: product.code,
        quantity: this.form.value.quantity!
      })
      .subscribe(() => {
        this.form.reset({ productId: '', quantity: 1 });
        this.saved.emit();
      });
  }

  removeItem(itemId: string) {
    const invoice = this.invoice();
    if (!invoice) return;

    this.invoiceService.removeItem(invoice.id, itemId).subscribe(() => {
      this.saved.emit();
    });
  }

  closeInvoice() {
    const invoice = this.invoice();
    if (!invoice) return;

    this.invoiceService.close(invoice.id).subscribe(() => {
      this.saved.emit();
    });
  }
}
