import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  http =inject(HttpClient);
  BaseUrl="https://localhost:5098";
  getUsers(){
    return this.http.get(this.BaseUrl+"/api/user");
  }
  
}
