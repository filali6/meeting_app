import { Component, inject, NgModule } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { AccountsService } from '../_services/accounts.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule,BsDropdownModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  model : any={};
  AccountService=inject(AccountsService);
  loging(){
    this.AccountService.login(this.model)
      .subscribe({
        next :response => console.log(response),
        error : err => console.log(err),
        complete:() => console.log("login completed")
      });
  }
  logout(){
    this.AccountService.logout();
    this.model={};
  }
}
