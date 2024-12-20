import { Component, inject } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-roles-model',
  imports: [],
  templateUrl: './roles-model.component.html',
  styleUrl: './roles-model.component.css'
})
export class RolesModelComponent {
bsModalRef=inject(BsModalRef);
username='';
title='d';
availableRoles : string[]=[];
SelectedRoles : string[]=[];
rolesUpdated =false;
updateCkecked(checkedValue:string){
  if(this.SelectedRoles.includes(checkedValue)){
    this.SelectedRoles=this.SelectedRoles.filter(r=>r!==checkedValue);
  }
  else{
    this.SelectedRoles.push(checkedValue);
  }
}

onSelectRoles(){
  this.rolesUpdated=true;
  this.bsModalRef.hide();
}
}
