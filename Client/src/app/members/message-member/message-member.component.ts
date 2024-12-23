import { Component, inject, input, OnInit, output } from '@angular/core';
import { Message } from '../../_models/Message';
import { MessageService } from '../../_services/message.service';
import { FormsModule } from '@angular/forms';
import { tap } from 'rxjs';
import { DatePipe } from '@angular/common';
import { TimeagoModule } from 'ngx-timeago';

@Component({
  selector: 'app-message-member',
  imports: [FormsModule,DatePipe,TimeagoModule],
  templateUrl: './message-member.component.html',
  styleUrl: './message-member.component.css'
})
export class MessageMemberComponent {

  username=input.required<string>();
  messageService=inject(MessageService);
  messageToSend:string|null=null;


  sendMessage(){
    if(this.messageToSend&&this.messageToSend?.length>0)
      this.messageService.sendMessage(this.username(),this.messageToSend!)
    .then( ()=>this.messageToSend="");
  }
}
