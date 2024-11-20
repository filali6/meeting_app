import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { Member } from '../../_models/Member';
import { MemberCardComponent } from "../member-card/member-card.component";

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardComponent],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit {
  private membersService=inject(MembersService);
  members!: Member[] | null;
  ngOnInit(): void {
    this.loadMembers()
  }
  loadMembers(){
    this.membersService.getMembers().subscribe({
      next : response =>{console.log(response);this.members=response;}
    });
  }
}
