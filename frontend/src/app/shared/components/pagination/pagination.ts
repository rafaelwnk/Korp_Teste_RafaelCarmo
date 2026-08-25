import { Component, computed, input, output } from '@angular/core';

@Component({
  imports: [],
  selector: 'app-pagination',
  templateUrl: './pagination.html',
})
export class Pagination {
  page = input.required<number>();
  totalPages = input.required<number>();

  pageChange = output<number>();

  pages = computed(() =>
    Array.from({ length: this.totalPages() }, (_, i) => i + 1)
  );

  goTo(page: number) {
    if (page < 1 || page > this.totalPages() || page === this.page()) return;
    this.pageChange.emit(page);
  }
}
