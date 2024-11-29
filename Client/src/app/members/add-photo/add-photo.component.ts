import { Component, inject, input, OnInit, output } from '@angular/core';
import { Photo } from '../../_models/Photo';
import { Member } from '../../_models/Member';
import { FileItem, FileUploader, FileUploadModule } from 'ng2-file-upload';
import { environment } from '../../../environments/environment';
import { AccountsService } from '../../_services/accounts.service';
import { DecimalPipe, NgClass, NgFor, NgIf, NgStyle } from '@angular/common';
import { MembersService } from '../../_services/members.service';

@Component({
  selector: 'app-add-photo',
  standalone: true,
  imports: [NgClass,NgFor,NgIf,NgStyle,FileUploadModule,DecimalPipe],
  templateUrl: './add-photo.component.html',
  styleUrl: './add-photo.component.css'
})
export class AddPhotoComponent implements OnInit{
  member=input.required<Member>();
  memberParent=output<Member>();
  uploader?:FileUploader;
  url=environment.apiUrl;
  hasBaseDropZoneOver =false;
  private memberService=inject(MembersService);
  private account=inject(AccountsService);
  ngOnInit(): void {
    this.uploader = new FileUploader({
      url:this.url+'user/add-photo',
      authToken: `Bearer ${this.account.currentUser()?.token}`,
      method : "POST",
      allowedFileType: ["image"],
      autoUpload : false,
      maxFileSize : 10*1024*1024,
      isHTML5 : true,
      
    });
    this.uploader.onBeforeUploadItem =(file)=>{
      file.withCredentials=false;
    };
    this.uploader.onSuccessItem=(file,response,status,headers)=>{
      const photo = JSON.parse(response);
      const updatedMember = {...this.member()};
      updatedMember.photos?.push(photo);
      this.memberParent.emit(updatedMember);
      this.uploader?.clearQueue();
    };
    
  }
  public fileOverBase(e:any):void {
    this.hasBaseDropZoneOver = e;
  }

  toMain(photo:Photo)
  {
    this.memberService.changeMainPhoto(photo);
    photo.isMain=true;
    const updatedMember = {...this.member()};
    updatedMember.photos?.map(p=>p.isMain=photo.id===p.id);
  }
  deletePhoto(photo:Photo)
  {
    this.memberService.deletePhoto(photo);
    const updatedMember = {...this.member()};
    updatedMember.photos=updatedMember.photos?.filter(p=>p.id!==photo.id);
    this.memberParent.emit(updatedMember);
  }
}
