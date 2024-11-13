import { Component, inject, Inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { NgFor } from '@angular/common';
import { NavComponent } from "./nav/nav.component";
import { UsersService } from './_services/users.service';
import { AccountsService } from './_services/accounts.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [NgFor, NavComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit  {
  private UserServices =inject(UsersService);
  private accountService= inject(AccountsService);
  title = 'DatingApp';
  users : any;
  ngOnInit(): void {
    this.getUsers();
    this.setCurrentUser();
  }
  getUsers(){
    this.UserServices.getUsers()
    .subscribe({
      next : response => this.users=response,
      error : err=>console.log(err),
      complete : ()=>console.log("completed")
    });
  }
  setCurrentUser(){
    var user=localStorage.getItem("user");
    if(!user) return
    this.accountService.currentUser.set(JSON.parse(user));
  }
}
