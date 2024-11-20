import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { MembersService } from '../../_services/members.service';
import { Member } from '../../_models/Member';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit {
  member? :Member ;
  rout = inject(ActivatedRoute);
  membersService=inject(MembersService);
  ngOnInit(): void {
      this.loadMember();
  }
  loadMember(){
    const username=this.rout.snapshot.paramMap.get("username");
    if(username){
      this.membersService.getMember(username)
                  .subscribe({
                    next : response=>this.member=response
                  })
    }
  }

}
