import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AccountsService } from '../_services/accounts.service';

export const addTokenInterceptor: HttpInterceptorFn = (req, next) => {
  let account = inject(AccountsService);
  if(account.currentUser())
  req=req.clone({
    setHeaders: { Authorization: `bearer ${account.currentUser()?.token}` }
  })
  return next(req);
};
