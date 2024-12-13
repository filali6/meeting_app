import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MembersService } from '../../_services/members.service';
import { Member } from '../../_models/Member';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TimeagoModule, TimeagoPipe } from 'ngx-timeago';
import { DatePipe } from '@angular/common';

@Component({
    selector: 'app-member-detail',
    imports: [GalleryModule,TimeagoModule,DatePipe],
    templateUrl: './member-detail.component.html',
    styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit {
  member? :Member ;
  rout = inject(ActivatedRoute);
  membersService=inject(MembersService);
  images: GalleryItem[]=[];
  ngOnInit(): void {
      this.loadMember();
      console.log(this.images);

  }
  loadMember(){
    const username=this.rout.snapshot.paramMap.get("username");
    if(username){
      this.membersService.getMember(username)
                  .subscribe({
                    next : response=>{this.member=response;
                      this.loadImages(response);
                    }
                  })
    }
  }
  loadImages(member:Member){
    member.photos?.forEach(element =>{ 
      this.images.push( new ImageItem({ src: `${element.url}`, thumb: `${element.url}` }));
      console.log(this.images);}
    );
  }

}
