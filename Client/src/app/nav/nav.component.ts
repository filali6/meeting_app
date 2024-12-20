import { Component, inject, NgModule } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { AccountsService } from '../_services/accounts.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { Toast, ToastrService } from 'ngx-toastr';
import { HasRoleDirective } from '../_directives/has-role.directive';

@Component({
    selector: 'app-nav',
    imports: [FormsModule, BsDropdownModule, RouterLink, RouterLinkActive,HasRoleDirective],
    templateUrl: './nav.component.html',
    styleUrl: './nav.component.css'
})
export class NavComponent {
  model : any={};
  toast=inject(ToastrService);
  AccountService=inject(AccountsService);
  loging(){
    this.AccountService.login(this.model)
      .subscribe({
        next :response => console.log(response),
        complete:() => console.log("login completed")
      });
  }
  logout(){
    this.AccountService.logout();
    this.model={};
  }
}
