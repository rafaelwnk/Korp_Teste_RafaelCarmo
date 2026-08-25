import { Component, effect, input, output } from '@angular/core';
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

  form = new FormGroup({
    description: new FormControl('', { nonNullable: true, validators: [Validators.required] })
  });

  constructor(private productService: ProductService) {
    effect(() => {
      const p = this.product();
      if (p) {
        this.form.patchValue({ description: p.description });
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
}
