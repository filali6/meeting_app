import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { Member } from '../../_models/Member';
import { MemberCardComponent } from "../member-card/member-card.component";
import { PaginationModule,PaginationComponent} from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardComponent,PaginationModule],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit {
   membersService=inject(MembersService);
   pageNumber =1;
   pageSize = 4;
  ngOnInit(): void {
  if(!this.membersService.PaginatedRes())this.loadMembers()
  }
  loadMembers(){
    this.membersService.getMembers(this.pageNumber,this.pageSize);
  }
  pageChanged(page:any){
    this.pageNumber=page.page;
    console.log(page);
    this.loadMembers();
  }
}
