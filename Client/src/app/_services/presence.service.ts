import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr'
import { ToastrService } from 'ngx-toastr';
import { UserRoles } from '../_models/UsersRoles';
import { User } from '../_models/User';
import { take } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  hubUrl=environment.hubsUrl;
  private hubConnection?:HubConnection;
  private router = inject(Router);
  private toast = inject(ToastrService);
  onlineUsers= signal<string[]>([]);
  creatHubConnection(user : User)
  {
    this.hubConnection=new HubConnectionBuilder()
                            .withUrl(this.hubUrl+'presence',{
                               accessTokenFactory:()=>user.token
                              })
                            .withAutomaticReconnect()
                            .build();
    this.hubConnection.start().catch(err=>console.log(err));
    this.hubConnection.on("UserIsOnline",username=>{
      this.onlineUsers.update(users=>[...users,username]);
    });
    this.hubConnection.on("UserIsOffline",username=>{
      this.onlineUsers.update(users=>users.filter(x=>x!==username));
    });
    this.hubConnection.on("GetOnlineUsers",currentUsers =>this.onlineUsers.set(currentUsers));

    this.hubConnection.on("NewMessageRecieved",({username,knownAs})=>{
      console.log("notif toaster");
      this.toast.info(knownAs+" sent you a message\nClick me to see it !")
      .onTap
      .pipe(take(1))
      .subscribe(()=>this.router.navigateByUrl("/member/"+username+"?tab=Messages"));
    })
  }
  stopHubConnection()
  {
    if(this.hubConnection?.state===HubConnectionState.Connected)
    {
      this.hubConnection.stop().catch(err=>console.log(err));
    }
  }
}
