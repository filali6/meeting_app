import { Component, inject } from '@angular/core';
import { RegisterFormComponent } from "../register-form/register-form.component";
import { UsersService } from '../_services/users.service';
import { AccountsService } from '../_services/accounts.service';

@Component({
    selector: 'app-home',
    imports: [RegisterFormComponent],
    templateUrl: './home.component.html',
    styleUrl: './home.component.css'
})
export class HomeComponent {

  registerMode=false;
  UserServices=inject(UsersService);
  AccountService=inject(AccountsService);
  RegisterToggle()
  {
    this.registerMode=!this.registerMode;
    //this.getUsers();
  }

}
