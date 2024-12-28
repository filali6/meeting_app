import { Component, inject, input, Input, OnInit, output } from '@angular/core';
import { Photo } from '../../_models/Photo';
import { AdminService } from '../../_services/admin.service';
import { GalleryModule, ImageItem, GalleryItem } from 'ng-gallery';

@Component({
  selector: 'app-photo-management',
  imports: [GalleryModule],
  templateUrl: './photo-management.component.html',
  styleUrl: './photo-management.component.css'
})
export class PhotoManagementComponent   {
  photos=input.required<Photo[]>();
  decision=output<any>();
  deletePhoto(photo: Photo) {
    this.decision.emit({approuve:false,photoId:photo.id});
  }
  Approuve(photo: Photo) {
    this.decision.emit({approuve:true,photoId:photo.id});
  }
  
}
