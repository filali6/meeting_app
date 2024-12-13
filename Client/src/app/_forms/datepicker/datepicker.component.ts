import { NgIf } from '@angular/common';
import { Component, input, Self } from '@angular/core';
import { FormControl, NgControl, ReactiveFormsModule } from '@angular/forms';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

@Component({
    selector: 'app-datepicker',
    imports: [BsDatepickerModule, ReactiveFormsModule, NgIf],
    templateUrl: './datepicker.component.html',
    styleUrl: './datepicker.component.css'
})
export class DatepickerComponent {
  label = input<string>('');

  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this
  }

  writeValue(obj: any): void {
  }

  registerOnChange(fn: any): void {
  }

  registerOnTouched(fn: any): void {
  }

  get control(): FormControl {
    return this.ngControl.control as FormControl
  }
}
