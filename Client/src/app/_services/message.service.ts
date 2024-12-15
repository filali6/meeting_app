import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { PaginationResults } from '../_models/Pagination';
import { Message } from '../_models/Message';
import { getParams, setPaginatedResp } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
baseUrl=environment.apiUrl;
private http=inject(HttpClient);
paginatedResult = signal<PaginationResults<Message[]>|null>(null);
getMessage(pageNumber:number,pageSize:number,container:string)
{
  let params = getParams(pageNumber,pageSize);
  params = params.append("Container",container);
  return this.http.get<Message[]>(this.baseUrl+"message",{observe :"response",params:params})
                  .subscribe({
                    next :response => setPaginatedResp(response,this.paginatedResult)
                  }) ; 
}
getMessageThread(username:string){
  return this.http.get<Message[]>(this.baseUrl+"message/thread/"+username);
}
sendMessage(username:string, content:string)
{
  const message={content:content,TargetUsername:username};
  return this.http.post<Message>(this.baseUrl+"message",message);
}
deleteMessage(messageId:number)
{
  return this.http.delete(this.baseUrl+"message/"+messageId);
}
}
