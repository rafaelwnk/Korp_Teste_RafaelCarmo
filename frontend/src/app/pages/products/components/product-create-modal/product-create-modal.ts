import { Component, output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProductService } from '../../services/product.service';
import { Modal } from "../../../../shared/components/modal/modal";

@Component({
  imports: [Modal, ReactiveFormsModule],
  selector: 'app-product-create-modal',
  templateUrl: './product-create-modal.html',
})
export class ProductCreateModal {
  created = output<void>();

  form = new FormGroup({
    code: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    description: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    stockBalance: new FormControl(0, { nonNullable: true, validators: [Validators.required, Validators.min(0)] })
  });

  constructor(private productService: ProductService) { }

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.productService.create(this.form.getRawValue()).subscribe(() => {
      this.form.reset({ code: '', description: '', stockBalance: 0 });
      this.created.emit();
    });
  }
}
