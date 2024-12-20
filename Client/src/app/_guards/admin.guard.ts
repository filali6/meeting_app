import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountsService } from '../_services/accounts.service';
import { ToastrService } from 'ngx-toastr';

export const adminGuard: CanActivateFn = (route, state) => {
  const accountService=inject(AccountsService);
  const toastr=inject(ToastrService);
  console.log(accountService.roles());
  if(accountService.roles().includes("admin")||accountService.roles().includes("Moderator"))return true;
  else toastr.error("aria not for you");
  return false;
};
