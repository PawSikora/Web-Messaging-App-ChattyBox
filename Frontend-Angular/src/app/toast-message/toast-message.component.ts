import { Component, Inject } from '@angular/core';
import { MAT_SNACK_BAR_DATA, MatSnackBarRef } from '@angular/material/snack-bar';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';



@Component({
  selector: 'app-toast-message',
  templateUrl: './toast-message.component.html',
  styleUrls: ['./toast-message.component.css']
})

export class ToastMessageComponent {
  
  constructor( @Inject(MAT_SNACK_BAR_DATA) public data: string, public snackBarRef: MatSnackBarRef<ToastMessageComponent>)
  { }

  
}
