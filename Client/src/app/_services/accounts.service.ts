import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { User } from '../_models/User';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountsService {

  http =inject(HttpClient);
  BaseUrl="https://localhost:5098/api/";
  currentUser=signal<User|null>(null);
  login(model:any){
    return this.http.post<User>(this.BaseUrl+"account/login",model).pipe(
      map(user=>{
        if(user){
          console.log("connected");
          localStorage.setItem("user",JSON.stringify(user));
          this.currentUser.set(user);
        }
      })
    );
  }
  logout(){
    this.currentUser.set(null);
    localStorage.clear();
  }
}
