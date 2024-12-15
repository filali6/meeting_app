import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/Member';
import { AccountsService } from './accounts.service';
import { asyncScheduler, of, scheduled } from 'rxjs';
import { Photo } from '../_models/Photo';
import { User } from '../_models/User';
import { PaginationResults } from '../_models/Pagination';
import { UserParams } from '../_models/UserParams';
import { getParams, setPaginatedResp } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  private accountService = inject(AccountsService);
  members = signal<Member[]>([]);
  memberCach = new Map();
  PaginatedRes = signal<PaginationResults<Member[]> | null>(null);
  getMembers(UserParams: UserParams) {
    const res = this.memberCach.get(Object.values(UserParams).join("-"));
    if (res) {
      return setPaginatedResp(res,this.PaginatedRes);
    }
    const params = this.getParams(UserParams);
    return this.http.get<Member[]>(this.baseUrl + "User", { observe: 'response', params: params })
      .subscribe({
        next: response => {
          setPaginatedResp(response,this.PaginatedRes);
          this.memberCach.set(Object.values(UserParams).join("-"), response);
        }
      });

  }

  getMember(username: string) {
    const member : Member=[...this.memberCach.values()].reduce((arr,elem)=>arr.concat(elem.body),[]).find((m:Member)=>m.userName===username);

    if (member) return of(member);
    return this.http.get<Member>(this.baseUrl + "User/" + username);
  }
  editMember(member: Member) {
    return this.http.put(this.baseUrl + "user", member);
  }
  changeMainPhoto(photo: Photo) {
    this.http.put(this.baseUrl + "User/" + photo.id, {}).subscribe({
      complete: () => {
        let memberAux = this.accountService.currentUser();
        memberAux!.url = photo.url;
        this.accountService.currentUser.set(memberAux);
        localStorage.setItem("user", JSON.stringify(memberAux));
        //this.updateMember(memberAux);
        this.members.update(members => {
          const memberIndex = members.findIndex(member => member.userName === this.accountService.currentUser()?.username);
          const updatedMembers = [...members];
          updatedMembers[memberIndex].photoUrl = photo.url;
          return updatedMembers;
        });
      }
    });
  }
  deletePhoto(photo: Photo) {
    this.http.delete(this.baseUrl + "User/" + photo.id, {}).subscribe({
      complete: () => {
        this.members.update(members => {
          const memberIndex = members.findIndex(member => member.userName === this.accountService.currentUser()?.username);
          const updatedMembers = [...members];
          updatedMembers[memberIndex].photos = updatedMembers[memberIndex].photos?.filter(p => p.id !== photo.id);
          return updatedMembers;
        });
      }
    });
  }
  updateMember(updatedMember: Member): void {
    if (!updatedMember) {
      console.error("Username cannot be null or undefined for update.");
      return;
    }

    this.members.update(currentMembers => {
      const memberIndex = currentMembers.findIndex(member => member.userName === updatedMember.userName);

      if (memberIndex === -1) {
        return currentMembers;
      }

      const updatedMembers = [...currentMembers];
      updatedMembers[memberIndex] = updatedMember;

      return updatedMembers;
    });
    console.log
  }
  private getParams(userparams: UserParams): HttpParams {
    let params = getParams(userparams.pageNumber,userparams.pageSize);
    if (userparams.gender)
      params = params.append("gender", userparams.gender);
    if (userparams.minAge)
      params = params.append("gender", userparams.minAge);
    if (userparams.maxAge)
      params = params.append("gender", userparams.maxAge);
    return params;
  }
}
