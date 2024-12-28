import { Component, inject, signal, ViewChild } from '@angular/core';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { UserManagementComponent } from "../user-management/user-management.component";
import { HasRoleDirective } from '../../_directives/has-role.directive';
import { PhotoManagementComponent } from "../photo-management/photo-management.component";
import { Photo } from '../../_models/Photo';
import { AdminService } from '../../_services/admin.service';

@Component({
  selector: 'app-admin-panel',
  imports: [TabsetComponent, UserManagementComponent, HasRoleDirective, TabsModule, PhotoManagementComponent],
  templateUrl: './admin-panel.component.html',
  styleUrl: './admin-panel.component.css'
})
export class AdminPanelComponent {
 @ViewChild("adminSets",{static:true})adminSets?:TabsetComponent;
  activeTab?:TabDirective;
  adminService = inject(AdminService);

  photosToApprouve=signal<Photo[]>([]);
  onTabActivated(data:TabDirective){
    this.activeTab=data;
    if(this.activeTab?.heading==="Photo management")
    {
      console.log("photo management tab");
      this.adminService.getPhotosUnapprouved().subscribe({
        next: response => this.photosToApprouve.set(response)
      });

    }
  }
  updatePhotos(photoId: number) {
    this.photosToApprouve.update(photos=>photos.filter(p => p.id !== photoId));
  }
  makeDecision(event:any)
  {
    if(event.approuve)
    {
      this.adminService.approuvePhoto(event.photoId).subscribe({
        next:_=>console.log("approuveed")
      })
    }
    else
    {
      console.log("del");
    }
    this.updatePhotos(event.photoId);
  }
}
