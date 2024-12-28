import { Component, inject, ModelOptions, OnInit } from '@angular/core';
import { AdminService } from '../../_services/admin.service';
import { UserRoles } from '../../_models/UsersRoles';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { RolesModelComponent } from '../../modals/roles-model/roles-model.component';

@Component({
  selector: 'app-user-management',
  imports: [],
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.css'
})
export class UserManagementComponent implements OnInit{
  private adminService = inject(AdminService);
  Users : UserRoles[]=[];
  private modalService = inject(BsModalService);
  bsModelRef:BsModalRef<RolesModelComponent>=new BsModalRef<RolesModelComponent>();
  ngOnInit(): void {
   this.getUsersWithRoles();
  }
  
  openRolesModel(user:UserRoles){
    const Initial:ModalOptions={
      class:"modal-lg",
      initialState:{
        username:user.username,
        title:"User Roles",
        SelectedRoles:[...user.roles],
        availableRoles : ['admin','Moderator','member'],
        user:this.Users,
        rolesUpdated:false
      }
    }
    this.bsModelRef=this.modalService.show(RolesModelComponent,Initial);
    this.bsModelRef.onHide?.subscribe({
      next:()=>{
        if(this.bsModelRef.content&&this.bsModelRef.content.rolesUpdated){
          const selected=this.bsModelRef.content.SelectedRoles;
          this.adminService.updateUserRolse(user.username,selected).subscribe({
            next:roles=>user.roles=roles
          })
        }
      }
    })
  }

  getUsersWithRoles(){
    this.adminService.getUserWithRoles().subscribe({
      next :response=>this.Users=response
    });
  }

}
