import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { User } from '../_models/User';
import { map } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';
import { UserRegister } from '../_models/UserRegister';
import { LikesService } from './likes.service';

@Injectable({
  providedIn: 'root'
})
export class AccountsService {
  private likeService = inject(LikesService);//carefull from circular dependancy when inject service into service
  http = inject(HttpClient);
  BaseUrl = environment.apiUrl+"account/";
  currentUser = signal<User | null>(null);
  router = inject(Router);
  login(model: any) {
    return this.http.post<User>(this.BaseUrl + "login", model).pipe(
      map(user => {
        if (user) {
          this.setCurrentUser(user);
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
  register(model: UserRegister) {

    return this.http.post<User>(this.BaseUrl + "register", model).pipe(
      map(user => {
        this.setCurrentUser(user);
        return user
      })
    );
  }
  setCurrentUser(user:User)
  {
    localStorage.setItem("user", JSON.stringify(user));
    this.currentUser.set(user);
    this.likeService.getLikeIds();
  }
}
