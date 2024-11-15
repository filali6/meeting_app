import { Component, inject, input, NgModule, output } from '@angular/core';
import { FormsModule, NgModel } from '@angular/forms';
import { AccountsService } from '../_services/accounts.service';
import { UsersInRegister } from '../_models/UsersInRegister';
import { NgFor } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register-form',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register-form.component.html',
  styleUrl: './register-form.component.css'
})
export class RegisterFormComponent {
model:any={};
toaster = inject(ToastrService);
usersFromHome = input.required<any>();
cancelRegister=output();
accountService=inject(AccountsService);
register()
{
    console.log("register model: ",this.model);
    this.accountService.register(this.model).subscribe(
    {
      next : response =>console.log("response: ",response),
      error : err=>{console.log("error");console.log(err);this.toaster.error(err.error);},
      complete : ()=>console.log("register completed")
    }
    );
    this.cancel();
}
cancel(){
  this.cancelRegister.emit();
}
}
