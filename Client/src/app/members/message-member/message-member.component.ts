import { Component, inject, input, OnInit, output } from '@angular/core';
import { Message } from '../../_models/Message';
import { MessageService } from '../../_services/message.service';
import { FormsModule } from '@angular/forms';
import { tap } from 'rxjs';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-message-member',
  imports: [FormsModule,DatePipe],
  templateUrl: './message-member.component.html',
  styleUrl: './message-member.component.css'
})
export class MessageMemberComponent {

  username=input.required<string>();
  messages=input.required<Message[]>();
  messageService=inject(MessageService);
  messageToSend:string|null=null;
  outputMessage=output<Message>()


  sendMessage(){
    if(this.messageToSend&&this.messageToSend?.length>0)
      this.messageService.sendMessage(this.username(),this.messageToSend!)
    .pipe(tap(_=>this.messageToSend=""))
    .subscribe({
    
    next : response=>this.outputMessage.emit(response) })
  }
}
