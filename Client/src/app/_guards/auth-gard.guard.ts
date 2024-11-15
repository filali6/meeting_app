import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountsService } from '../_services/accounts.service';

export const authGardGuard: CanActivateFn = (route, state) => {
  const toast = inject(ToastrService);
  const accountService= inject(AccountsService);
  const router= inject(Router);
  if(accountService.currentUser()) return true;
  router.navigateByUrl("");
  toast.error("ressource needs authentificatiobn!")
  return false;
};
