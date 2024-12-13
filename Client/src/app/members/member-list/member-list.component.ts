import { Component, inject, NgModule, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { Member } from '../../_models/Member';
import { MemberCardComponent } from "../member-card/member-card.component";
import { PaginationModule,PaginationComponent} from 'ngx-bootstrap/pagination';
import { UserParams } from '../../_models/UserParams';
import { FiltersComponent } from "../../filters/filters.component";
import { FormsModule, NgModel } from '@angular/forms';

@Component({
    selector: 'app-member-list',
    imports: [MemberCardComponent, PaginationModule, FiltersComponent,FormsModule],
    templateUrl: './member-list.component.html',
    styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit {

   membersService=inject(MembersService);
   userParams : UserParams={
     pageNumber: 1,
     pageSize: 4,
     minAge: 18,
     maxAge: 80,

   };
  ngOnInit(): void {
  if(!this.membersService.PaginatedRes())this.loadMembers()
  }
  loadMembers(){
    console.log("userpar:",this.userParams);
    this.membersService.getMembers(this.userParams);
  }
  pageChanged(page:any){
    this.userParams.pageNumber=page.page;
    console.log(page);
    this.loadMembers();
  }
  getFilter($event: UserParams) {
    this.userParams=$event;
    this.loadMembers();
    }
}
