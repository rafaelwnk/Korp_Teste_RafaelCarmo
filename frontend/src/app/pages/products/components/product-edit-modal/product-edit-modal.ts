import { Component, effect, input, output, signal } from '@angular/core';
import { Modal } from '../../../../shared/components/modal/modal';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Product } from '../../models/product.model';
import { ProductService } from '../../services/product.service';

@Component({
  imports: [Modal, ReactiveFormsModule],
  selector: 'app-product-edit-modal',
  templateUrl: './product-edit-modal.html',
})
export class ProductEditModal {
  product = input<Product | undefined>(undefined);
  saved = output<void>();

  currentStock = signal<number>(0);

  form = new FormGroup({
    description: new FormControl('', { nonNullable: true, validators: [Validators.required] })
  });

  stockForm = new FormGroup({
    quantity: new FormControl(1, { nonNullable: true, validators: [Validators.required, Validators.min(1)] })
  });

  constructor(private productService: ProductService) {
    effect(() => {
      const p = this.product();
      if (p) {
        this.form.patchValue({ description: p.description });
        this.currentStock.set(p.stockBalance);
      }
    });
  }

  save() {
    const p = this.product();
    if (!p || this.form.invalid) return;

    this.productService
      .updateDescription(p.id, { description: this.form.value.description! })
      .subscribe(() => this.saved.emit());
  }

  increaseStock() {
    const p = this.product();
    if (!p || this.stockForm.invalid) return;

    const quantity = this.stockForm.value.quantity!;

    this.productService
      .increaseStock(p.id, { quantity })
      .subscribe((result) => {
        this.currentStock.set(result.data!.stockBalance);
        this.stockForm.reset({ quantity: 1 });
        this.saved.emit();
      });
  }

  decreaseStock() {
    const p = this.product();
    if (!p || this.stockForm.invalid) return;

    const quantity = this.stockForm.value.quantity!;

    this.productService
      .decreaseStock(p.id, { quantity })
      .subscribe((result) => {
        this.currentStock.set(result.data!.stockBalance);
        this.stockForm.reset({ quantity: 1 });
        this.saved.emit();
      });
  }
}