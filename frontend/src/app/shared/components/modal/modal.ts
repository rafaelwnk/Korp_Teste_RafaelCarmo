import { Component, input } from '@angular/core';

@Component({
  imports: [],
  selector: 'app-modal',
  templateUrl: './modal.html'
})
export class Modal {
  modalId = input.required<string>();
  title = input('');
}