import { inject, Injectable } from '@angular/core';
import { NgxSpinner, NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class SpinnerService {

  private spinner = inject(NgxSpinnerService);
  requestCount=0;
  on(){
    this.requestCount+=1;
    this.spinner.show();
  }

  off(){
    this.requestCount-=1;
    if(this.requestCount<0)this.requestCount=0;
    this.spinner.hide();
  }
  
}
