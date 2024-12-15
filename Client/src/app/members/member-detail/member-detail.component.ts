import { Component, inject, OnInit, ViewChild, viewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MembersService } from '../../_services/members.service';
import { Member } from '../../_models/Member';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TimeagoModule, TimeagoPipe } from 'ngx-timeago';
import { DatePipe } from '@angular/common';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { MessageMemberComponent } from "../message-member/message-member.component";
import { Message } from '../../_models/Message';
import { MessageService } from '../../_services/message.service';

@Component({
    selector: 'app-member-detail',
    imports: [GalleryModule, TimeagoModule, DatePipe, TabsModule, MessageMemberComponent],
    templateUrl: './member-detail.component.html',
    styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit {
  @ViewChild("memberSet",{static:true})memberSet?:TabsetComponent;
  activeTab?:TabDirective;
  messages:Message[]=[];
  messageService = inject(MessageService);
  member :Member ={} as Member;
  rout = inject(ActivatedRoute);
  membersService=inject(MembersService);
  images: GalleryItem[]=[];
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
      })
    this.rout.queryParams.subscribe({
      next: params=>{
        params['tab']&&this.selectTab(params['tab'])
      }
    })
  }
  onTabActivated(data:TabDirective){
    this.activeTab=data;
    if(this.activeTab.heading==="Messages" && this.messages.length===0 && this.member)
      this.messageService.getMessageThread(this.member.userName!).subscribe({
        next : response => this.messages=response
      })
  }
  // loadMember(){
  //   const username=this.rout.snapshot.paramMap.get("username");
  //   if(username){
  //     this.membersService.getMember(username)
  //                 .subscribe({
  //                   next : response=>{this.member=response;
  //                     this.loadImages(response);
  //                   }
  //                 })
  //   }
  // }
  loadImages(member:Member){
    member.photos?.forEach(element =>{ 
      this.images.push( new ImageItem({ src: `${element.url}`, thumb: `${element.url}` }));
      console.log(this.images);}
    );
  }
  addMessage(message : Message)
  {
    this.messages.push(message);
  }
  selectTab(heading:string){
    if(this.memberSet){
      const messageTab=this.memberSet.tabs.find(x=>x.heading===heading);
      if(messageTab)messageTab.active=true;
    }
  }
}
