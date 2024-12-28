import { Component, inject } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-confirm-dialog',
  imports: [],
  templateUrl: './confirm-dialog.component.html',
  styleUrl: './confirm-dialog.component.css'
})
export class ConfirmDialogComponent {
  bsModelRef = inject(BsModalRef);
  title = "";
  message = "";
  btnOkText = "";
  btnCnacelText = "";
  result = false;
  confirm() {
    this.result = true;
    this.bsModelRef.hide();
  }
  decline() {
    this.bsModelRef.hide();
  }
}
