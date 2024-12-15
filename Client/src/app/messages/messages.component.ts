import { Component, inject, OnInit } from '@angular/core';
import { MessageService } from '../_services/message.service';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { FormsModule } from '@angular/forms';
import { Message } from '../_models/Message';
import { RouterLink } from '@angular/router';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { DatePipe } from '@angular/common';

@Component({
    selector: 'app-messages',
    imports: [ButtonsModule,FormsModule,RouterLink,PaginationModule,DatePipe],
    templateUrl: './messages.component.html',
    styleUrl: './messages.component.css'
})
export class MessagesComponent implements OnInit{
messageService=inject(MessageService);
container = "unread";
pageNumber=1;
pageSize=5;
isOutbox=this.container==="outbox";
ngOnInit(): void {
    this.loadMessages();
}
loadMessages()
{
    this.messageService.getMessage(this.pageNumber,this.pageSize,this.container);
}
pageChanged(page:any){
    this.pageNumber=page.page;
    this.loadMessages();
  }
  getRout(message:Message){
    if(this.container==="outbox") return `/member/${message.targetUsername}`;
    else return `/member/${message.sourceUsername}`;
  }
  deleteMessages(id:number){
    this.messageService.deleteMessage(id).subscribe({
      next:()=>{
        this.messageService.paginatedResult.update(pr=>{
          if(pr && pr.items){
            pr.items=pr.items.filter(m=>m.id!==id);  
          }
          return pr;
        })
      }
    })
  }
}
