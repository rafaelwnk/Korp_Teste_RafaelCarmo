import { Component, input, output } from '@angular/core';

@Component({
  imports: [],
  selector: 'app-confirm-modal',
  templateUrl: './confirm-modal.html'
})
export class ConfirmModal {
  modalId = input.required<string>();
  title = input('Confirmar ação');
  message = input('Tem certeza que deseja continuar?');

  confirmed = output<void>();
}