import { Component, inject, OnInit, output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { AccountsService } from '../_services/accounts.service';
import { ToastrService } from 'ngx-toastr';
import { JsonPipe } from '@angular/common';
import { TextInputComponent } from "../_forms/text-input/text-input.component";
import { DatepickerComponent } from "../_forms/datepicker/datepicker.component";
import { UserRegister } from '../_models/UserRegister';
@Component({
    selector: 'app-register-form',
    imports: [ReactiveFormsModule, TextInputComponent, DatepickerComponent],
    templateUrl: './register-form.component.html',
    styleUrl: './register-form.component.css'
})
export class RegisterFormComponent implements OnInit{

model:any={};
fn =inject(FormBuilder);
toaster = inject(ToastrService);
cancelRegister=output();
accountService=inject(AccountsService);
formControl : FormGroup=new FormGroup({});
ngOnInit(): void {
  this.initialiseFormControle();
}
initialiseFormControle(){
  this.formControl=this.fn.group({
    username : ["",[Validators.required,Validators.maxLength(12)]],
    knownAs : ["",[Validators.required]],
    isMale : [true,[Validators.required]],
    country : ["",[Validators.required]],
    city : ["",[Validators.required]],
    introduction : ["",[Validators.required]],
    interests : ["",[Validators.required]],
    lookingFor : ["",[Validators.required]],
    password : ["",[Validators.required,Validators.minLength(4),Validators.pattern('[0-9]*')]],
    confirmPassword : ["",[this.validatorPassword("password")]],
    dateBirth : ["",[Validators.required]],
  })
  this.formControl.controls["password"].valueChanges.subscribe({
    next:_=>this.formControl.controls["confirmPassword"].updateValueAndValidity()
  });
}
validatorPassword(toMatch:string):ValidatorFn{
  return (control: AbstractControl)=>control.value!==control.parent?.get(toMatch)?.value ? {isMatching:true}:null;
}
register()
{
  
  let user :UserRegister={
    username: this.formControl.value.username,
    knownAs:this.formControl.value.knownAs,
    isMale:this.formControl.value.isMale,
    password: this.formControl.value.password,
    dateBirth:this.formControl.value.dateBirth.toISOString().substring(0,10), 
    introduction:this.formControl.value.introduction, 
    interests: this.formControl.value.interests,
    lookingFor: this.formControl.value.lookingFor, 
    city: this.formControl.value.city,
    country:this.formControl.value.country
  };
  console.log(user);
  //user.dateBirth=user.dateBirth.toISOString();
  
    this.accountService.register(user).subscribe(
    {
      next : response =>console.log("response: ",response),
      complete : ()=>console.log("register completed")
    }
    );
    this.cancel();
}
cancel(){
  this.cancelRegister.emit();
}
}
