import { CanDeactivateFn } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

export const editGuardGuard: CanDeactivateFn<MemberEditComponent> = (component) => {
  if(component.form?.dirty)return confirm("Changes has been made, if you continue they will be lost");
  return true;
};
