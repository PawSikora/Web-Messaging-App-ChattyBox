import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ToastMessageComponent } from '../toast-message/toast-message.component';

@Injectable({
  providedIn: 'root'
})
export class ToastMessageService {

  durationInMilliseconds = 3000;

  constructor(private _snackBar: MatSnackBar) { }

  notifyOfError(message: string) {
    this._snackBar.openFromComponent(ToastMessageComponent, {
      duration: this.durationInMilliseconds,
      data: message,
      panelClass: 'error-snackbar'
    });
  }

  notifyOfSuccess(message: string) {
    this._snackBar.openFromComponent(ToastMessageComponent, {
      duration: this.durationInMilliseconds,
      data: message,
      panelClass: 'success-snackbar'
    });
  }
  
}
