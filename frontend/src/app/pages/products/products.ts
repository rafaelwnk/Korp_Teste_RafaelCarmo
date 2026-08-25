import { Component, signal } from '@angular/core';
import { ProductService } from './services/product.service';
import { Product } from './models/product.model';
import { ProductDetailsModal } from "./components/product-details-modal/product-details-modal";
import { ProductEditModal } from "./components/product-edit-modal/product-edit-modal";
import { ConfirmModal } from "../../shared/components/confirm-modal/confirm-modal";
import { ProductCreateModal } from "./components/product-create-modal/product-create-modal";
import { Pagination } from "../../shared/components/pagination/pagination";

@Component({
  imports: [ProductDetailsModal, ProductEditModal, ConfirmModal, ProductCreateModal, Pagination],
  selector: 'app-products',
  templateUrl: './products.html',
})
export class Products {
  selectedProduct = signal<Product | undefined>(undefined);

  constructor(protected productService: ProductService) { }

  openDetails(product: Product) {
    this.selectedProduct.set(product);
  }

  openEdit(product: Product) {
    this.selectedProduct.set(product);
  }

  deleteProduct() {
    const product = this.selectedProduct();
    if (!product) return;

    this.productService.delete(product.id).subscribe(() => {
      this.productService.reload();
    });
  }
}
