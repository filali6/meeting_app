import { Component, inject, OnDestroy, OnInit, ViewChild, viewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MembersService } from '../../_services/members.service';
import { Member } from '../../_models/Member';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TimeagoModule, TimeagoPipe } from 'ngx-timeago';
import { DatePipe } from '@angular/common';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { MessageMemberComponent } from "../message-member/message-member.component";
import { Message } from '../../_models/Message';
import { MessageService } from '../../_services/message.service';
import { PresenceService } from '../../_services/presence.service';
import { AccountsService } from '../../_services/accounts.service';
import { HubConnectionState } from '@microsoft/signalr';

@Component({
    selector: 'app-member-detail',
    imports: [GalleryModule, TimeagoModule, DatePipe, TabsModule, MessageMemberComponent],
    templateUrl: './member-detail.component.html',
    styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit,OnDestroy {
  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }
  @ViewChild("memberSet",{static:true})memberSet?:TabsetComponent;
  activeTab?:TabDirective;
  messageService = inject(MessageService);
  member :Member ={} as Member;
  rout = inject(ActivatedRoute);
  private accountService=inject(AccountsService);
  presenceService=inject(PresenceService);
  images: GalleryItem[]=[];
  private router = inject(Router);
  ngOnInit(): void {
      this.rout.data.subscribe({
        next : response=>{
          console.log("memberdetai:");
          console.log(response);
          this.member=response[0];
          console.log("mm");
          console.log(this.member);
          if(this.member)
          this.loadImages(this.member);
        }
      });
      this.rout.paramMap.subscribe({
        next: _=>this.OnRoutPamasChange()
      })
    this.rout.queryParams.subscribe({
      next: params=>{
        params['tab']&&this.selectTab(params['tab'])
      }
    })
  }
  onTabActivated(data:TabDirective){
    this.activeTab=data;
    this.router.navigate([],{
      relativeTo:this.rout,
      queryParams:{tab:this.activeTab.heading},
      queryParamsHandling:"merge"
    });
    if(this.activeTab.heading==="Messages"  && this.member)
    {
      const user = this.accountService.currentUser();
      if(!user || !this.member.userName)return;
      console.log("creat hub message");
      this.messageService.creatHubConnection(user,this.member.userName)
    }
    else{
      this.messageService.stopHubConnection();
    }
  }
  OnRoutPamasChange()
  {
    const user = this.accountService.currentUser();
    if(!user)return;
    if(this.messageService.hubConnection?.state===HubConnectionState.Connected && this.activeTab?.heading==='Messages')
    {
      this.messageService.hubConnection.stop().then(()=>{
        this.messageService.creatHubConnection(user,this.member.userName!);
      })
    }

  }
  loadImages(member:Member){
    member.photos?.forEach(element =>{ 
      this.images.push( new ImageItem({ src: `${element.url}`, thumb: `${element.url}` }));
      console.log(this.images);}
    );
  }
  selectTab(heading:string){
    if(this.memberSet){
      const messageTab=this.memberSet.tabs.find(x=>x.heading===heading);
      if(messageTab)messageTab.active=true;
    }
  }
}
