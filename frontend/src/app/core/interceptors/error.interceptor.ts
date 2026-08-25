import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toastr = inject(ToastrService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      const message = error.error?.message || getDefaultMessage(error.status);
      toastr.error(message, 'Erro');
      return throwError(() => error);
    })
  );
};

function getDefaultMessage(status: number): string {
  switch (status) {
    case 0:
      return 'Não foi possível conectar ao servidor.';
    case 400:
      return 'Requisição inválida.';
    case 401:
      return 'Você precisa estar autenticado.';
    case 403:
      return 'Você não tem permissão para essa ação.';
    case 404:
      return 'Recurso não encontrado.';
    case 500:
      return 'Erro interno do servidor.';
    default:
      return 'Ocorreu um erro inesperado.';
  }
}