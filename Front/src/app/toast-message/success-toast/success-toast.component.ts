import { Component, Inject } from '@angular/core';
import { MAT_SNACK_BAR_DATA, MatSnackBarRef } from '@angular/material/snack-bar';

@Component({
  selector: 'app-success-toast',
  templateUrl: './success-toast.component.html',
  styleUrls: ['./success-toast.component.css']
})
export class SuccessToastComponent {
  constructor(@Inject(MAT_SNACK_BAR_DATA) public data: string,
  public snackBarRef: MatSnackBarRef<SuccessToastComponent>,) { }
}
