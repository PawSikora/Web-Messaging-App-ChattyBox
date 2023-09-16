import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { User } from 'src/app/interfaces/users-interfaces';
import { ChatService } from 'src/app/services/chat.service';

@Component({
  selector: 'app-choose-admin-modal',
  templateUrl: './choose-admin-modal.component.html',
  styleUrls: ['./choose-admin-modal.component.css']
})
export class ChooseAdminModalComponent {
  displayedUsers: User[] = [];
  pageSize = 5;
  usersPageNumber = 1;
  pages: number[] = [];

  constructor(
    public dialogRef: MatDialogRef<ChooseAdminModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { chatId: number, currentAdminId: number, users: User[] }) {
      this.data.users = this.data.users.filter(user => user.id !== this.data.currentAdminId);
      this.displayedUsers = this.data.users.slice(0, this.pageSize);
      
      let howManyPages = Math.ceil(data.users.length / this.pageSize);
      for (let i = 1; i <= howManyPages; i++) {
        this.pages.push(i);
      }
    }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onYesClick(chosenUser: User): void {
    this.dialogRef.close(chosenUser);
  }

  onPageChange(pageNumber: number): void {
    this.usersPageNumber = pageNumber;
    const startIndex = (pageNumber - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.displayedUsers = this.data.users.slice(startIndex, endIndex);
  }
}
