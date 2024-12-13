import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { LikesService } from '../_services/likes.service';
import { Member } from '../_models/Member';
import { FormsModule } from '@angular/forms';
import {ButtonsModule} from 'ngx-bootstrap/buttons'
import { MemberCardComponent } from "../members/member-card/member-card.component";
import { tap } from 'rxjs';
import { PaginationModule } from 'ngx-bootstrap/pagination';
@Component({
    selector: 'app-lists',
    imports: [ButtonsModule, FormsModule, MemberCardComponent,PaginationModule],
    templateUrl: './lists.component.html',
    styleUrl: './lists.component.css'
})
export class ListsComponent implements OnInit, OnDestroy {
    ngOnDestroy(): void {
        this.likeService.paginatedRes.set(null);
    }
     likeService=inject(LikesService);
    members:Member[]=[];
    predicate='like';
    pageNumber=1;
    pageSize=4;
    ngOnInit(): void {
        this.loadLikes();
    }
    loadLikes(){
        this.likeService.getLikes(this.predicate,this.pageNumber,this.pageSize);
    }
    getTitle(){
        switch(this.predicate){
            case 'like': return "members you like";
            case 'likedBy' : return "members that like you";
            default: return "members with mutual liking";
        }
    }
    pageChanged(page:any){
        this.pageNumber=page.page;
        this.loadLikes();
      }
}
