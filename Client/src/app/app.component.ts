import { Component, inject, Inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { NgFor } from '@angular/common';
import { NavComponent } from "./nav/nav.component";
import { UsersService } from './_services/users.service';
import { AccountsService } from './_services/accounts.service';
import { HomeComponent } from "./home/home.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,NavComponent],
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
    this.accountService.currentUser.set(JSON.parse(user));
  }
}
