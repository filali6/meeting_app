import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { SpinnerService } from '../_services/spinner.service';
import { finalize, tap } from 'rxjs';

export const spinnerInterceptor: HttpInterceptorFn = (req, next) => {
  const spinner=inject(SpinnerService);
  spinner.on();
  console.log(req);
  return next(req).pipe(finalize(
    ()=>spinner.off()
  ));
};
