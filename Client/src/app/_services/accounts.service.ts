import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { User } from '../_models/User';
import { map } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountsService {

  http = inject(HttpClient);
  BaseUrl = environment.apiUrl+"account/";
  currentUser = signal<User | null>(null);
  router = inject(Router);
  login(model: any) {
    return this.http.post<User>(this.BaseUrl + "login", model).pipe(
      map(user => {
        if (user) {
          localStorage.setItem("user", JSON.stringify(user));
          this.currentUser.set(user);
          this.router.navigateByUrl('members')
        }
      })
    );
  }
  logout() {
    this.currentUser.set(null);
    localStorage.clear();
    this.router.navigateByUrl('')
  }
  register(model: any) {
    var auxModel: any = {};
    auxModel.password = model.password;
    auxModel.username = model.username;
    return this.http.post<User>(this.BaseUrl + "/egister", auxModel).pipe(
      map(user => {
        localStorage.setItem("user", JSON.stringify(user));
        this.currentUser.set(user);
        return user
      })
    );
  }
}
