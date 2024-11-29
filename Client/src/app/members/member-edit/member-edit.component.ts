import { Component, inject, OnInit, ViewChild, viewChild } from '@angular/core';
import { Member } from '../../_models/Member';
import { MembersService } from '../../_services/members.service';
import { AccountsService } from '../../_services/accounts.service';
import { FormsModule, NgForm } from '@angular/forms';
import { Toast, ToastrService } from 'ngx-toastr';
import { AddPhotoComponent } from "../add-photo/add-photo.component";
import { TabsModule } from 'ngx-bootstrap/tabs';
@Component({
  selector: 'app-member-edit',
  standalone: true,
  imports: [FormsModule, AddPhotoComponent ,TabsModule],
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.css'
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') form?: NgForm;
  member?: Member;
  public accountSevice = inject(AccountsService);
  private membberService = inject(MembersService);
  private toastr = inject(ToastrService);
  ngOnInit(): void {
    this.loadMember();
  }
  loadMember() {
    const username = this.accountSevice.currentUser()?.username;
    if (username) {
      this.membberService.getMember(username).subscribe({
        next: response => this.member = response
      })
    }
  }
  editProfile() {

    this.membberService.editMember(this.form?.value).subscribe({
      next: _ => {
        this.form?.reset(this.form.value);
        this.toastr.success("profile edited");
      }
    });
  }
  memberChanged(event : Member){
    this.member=event;
  }

}
