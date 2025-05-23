import { Component, inject, Inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { NgFor } from '@angular/common';
import { NavComponent } from "./nav/nav.component";
import { UsersService } from './_services/users.service';
import { AccountsService } from './_services/accounts.service';
import { HomeComponent } from "./home/home.component";
import { NgxSpinnerModule } from 'ngx-spinner';

@Component({
  standalone:true,
    selector: 'app-root',
    imports: [RouterOutlet, NavComponent, NgxSpinnerModule],
    templateUrl: './app.component.html',
    styleUrl: './app.component.css'
})
export class AppComponent implements OnInit  {
  private UserServices =inject(UsersService);
  public accountService= inject(AccountsService);
  title = 'DatingApp';
  users : any;
  ngOnInit(): void {
    //this.getUsers();
    this.setCurrentUser();
  }
  
  setCurrentUser(){
    var user=localStorage.getItem("user");
    if(!user) return
    this.accountService.setCurrentUser(JSON.parse(user));
  }
}
