import { Component, input } from '@angular/core';
import { Product } from '../../models/product.model';
import { Modal } from '../../../../shared/components/modal/modal';
import { DatePipe } from '@angular/common';

@Component({
  imports: [Modal, DatePipe],
  selector: 'app-product-details-modal',
  templateUrl: './product-details-modal.html',
})
export class ProductDetailsModal {
  product = input<Product | undefined>(undefined);
}
