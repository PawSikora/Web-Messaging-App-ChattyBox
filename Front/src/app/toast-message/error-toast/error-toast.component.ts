import { Component, Inject } from '@angular/core';
import { MAT_SNACK_BAR_DATA, MatSnackBarRef } from '@angular/material/snack-bar';
import { SuccessToastComponent } from '../success-toast/success-toast.component';

@Component({
  selector: 'app-error-toast',
  templateUrl: './error-toast.component.html',
  styleUrls: ['./error-toast.component.css']
})
export class ErrorToastComponent {
  constructor(@Inject(MAT_SNACK_BAR_DATA) public data: string,
  public snackBarRef: MatSnackBarRef<SuccessToastComponent>,) { }
}
