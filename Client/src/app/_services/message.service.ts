import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { PaginationResults } from '../_models/Pagination';
import { Message } from '../_models/Message';
import { getParams, setPaginatedResp } from './paginationHelper';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { User } from '../_models/User';
import { group } from '@angular/animations';
import { Group } from '../_models/Group';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  paginatedResult = signal<PaginationResults<Message[]> | null>(null);
  messageThread = signal<Message[]>([]);
  //creat hub connextion
  hubUrl = environment.hubsUrl;
  hubConnection?: HubConnection;
  creatHubConnection(user: User, username: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'message?user=' + username, {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();
    this.hubConnection.start().catch(err => console.log(err));
    this.hubConnection.on("RecieveMessageThread", messages => {
      this.messageThread.set(messages);
    });
    this.hubConnection.on("NewMessage", message => {
      console.log("new message got:");
      console.log(message);
      this.messageThread.update(mt => [...mt, message]);
    });
    this.hubConnection.on("updatedGroup", (group: Group) => {
      if (group.connections.some(x => x.username === username)) {
        this.messageThread.update(th => {
          th.forEach(message => {
            if (!message.readDate) {
              message.readDate = new Date(Date.now());
            }
          });
          return th;
        });
      }
    });
  }
  stopHubConnection() {
    if (this.hubConnection?.state === HubConnectionState.Connected) {
      this.hubConnection.stop().catch(err => console.log(err));
    }
  }
  getMessage(pageNumber: number, pageSize: number, container: string) {
    let params = getParams(pageNumber, pageSize);
    params = params.append("Container", container);
    return this.http.get<Message[]>(this.baseUrl + "message", { observe: "response", params: params })
      .subscribe({
        next: response => setPaginatedResp(response, this.paginatedResult)
      });
  }

  async sendMessage(username: string, content: string) {
    const message = { content: content, TargetUsername: username };
    console.log("send");
    return this.hubConnection?.invoke("SendMessage", message);
  }
  deleteMessage(messageId: number) {
    return this.http.delete(this.baseUrl + "message/" + messageId);
  }
}
