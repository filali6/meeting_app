import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Member } from '../_models/Member';
import { PaginationResults } from '../_models/Pagination';
@Injectable({
  providedIn: 'root'
})
export class LikesService {
  baseUrl=environment.apiUrl+'like';
  private http=inject(HttpClient);
  likeIds = signal<number[]>([]);
  paginatedRes=signal<PaginationResults<Member[]>|null>(null);
  toggleLike(targetId:number){
    return this.http.post(this.baseUrl+"/"+targetId,{});
  }
  getLikes(predicate:string,pageNumber:number,pageSize:number){
      let params = new HttpParams();
      params = params.append("predicate",predicate);
      params = params.append("pageNumber", pageNumber);
      params = params.append("PageSize", pageSize);
        
    return this.http.get<Member[]>(this.baseUrl,{
      observe:"response",
      params:params
    }).subscribe({
      next:res=>{
        this.paginatedRes.set({
          items: res.body as Member[],
          Pagination: JSON.parse(res.headers.get("Pagination")!)
      })
      }
    });
  }
  getLikeIds(){
    return this.http.get<number[]>(this.baseUrl+"/list").subscribe({
      next: ids=>this.likeIds.set(ids)
    });
  }
}
