import { Component, inject, OnInit, output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { UserParams } from '../_models/UserParams';

@Component({
    selector: 'app-filters',
    imports: [ReactiveFormsModule],
    templateUrl: './filters.component.html',
    styleUrl: './filters.component.css'
})
export class FiltersComponent implements OnInit {

  fb=inject(FormBuilder);
  filterForm: FormGroup=new FormGroup({});
  params=output<UserParams>();
  ngOnInit(): void {
    this.initializeForm();  
  }
  initializeForm(){
    this.filterForm=this.fb.group({
      pageSize:["4"],
      minAge:["18"],
      maxAge:["80"],
      gender:["Male"],
    })
  }
  filtrer(){
    const userParams : UserParams={
      pageNumber: 1,
      pageSize: this.filterForm.value.pageSize,
      minAge: this.filterForm.value.minAge,
      maxAge: this.filterForm.value.maxAge,
      gender: this.filterForm.value.gender,
    }
    this.params.emit(userParams);
  }

}
