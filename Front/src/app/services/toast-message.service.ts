import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ErrorToastComponent } from '../toast-message/error-toast/error-toast.component';
import { SuccessToastComponent } from '../toast-message/success-toast/success-toast.component';

@Injectable({
  providedIn: 'root'
})
export class ToastMessageService {

  durationInMilliseconds = 3000;

  constructor(private _snackBar: MatSnackBar) { }

  notifyOfError(message: string) {
    this._snackBar.openFromComponent(ErrorToastComponent, {
      duration: this.durationInMilliseconds,
      data: message,
    });
  }

  notifyOfSuccess(message: string) {
    this._snackBar.openFromComponent(SuccessToastComponent, {
      duration: this.durationInMilliseconds,
      data: message,
    });
  }
  
}
