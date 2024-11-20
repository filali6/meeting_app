import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  http =inject(HttpClient);
  BaseUrl=environment.apiUrl;
  
  
}
