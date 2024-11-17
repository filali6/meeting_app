import { Component, inject } from '@angular/core';
import { RegisterFormComponent } from "../register-form/register-form.component";
import { UsersService } from '../_services/users.service';
import { AccountsService } from '../_services/accounts.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterFormComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

  registerMode=false;
  users:any={};
  UserServices=inject(UsersService);
  AccountService=inject(AccountsService);
  RegisterToggle()
  {
    this.registerMode=!this.registerMode;
    this.getUsers();
  }
  getUsers(){
    if(!this.users)
    this.UserServices.getUsers()
    .subscribe({
      next : response => this.users=response,
      error : err=>console.log(err),
      complete : ()=>console.log("getUsers completed")
    });
  }
}
