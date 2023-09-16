import { Component, EventEmitter, Input, Output } from '@angular/core';
import { User } from 'src/app/interfaces/users-interfaces';
import { ChatService } from 'src/app/services/chat.service';

@Component({
  selector: 'app-search-user-modal',
  templateUrl: './search-user-modal.component.html',
  styleUrls: ['./search-user-modal.component.css']
})

export class SearchUserModalComponent {
  @Input() chatId!: number;
  @Input() chatUsers!: User[];
  @Output() closeSearchModal = new EventEmitter<boolean>();
  userId?: number;
  searchUser!: string;
  searchedUser?: User;
  addedUser: boolean = false;
  searchText: string = '';
  

  constructor(private chatService: ChatService) {}

  isUserInChat(userId:number):boolean{
    let isInChat:boolean = false;
    this.chatUsers.forEach((user)=>{
      if(user.id===userId){
        isInChat=true;
      }
    });
    return isInChat;
  }

  addUserToChat(userId:number, chatId:number):void{
    if(this.chatId)
    this.chatService.addUser(this.chatId, userId).subscribe(()=>{
      console.log("Dodano użytkownika do czatu");
      this.addedUser = true;
      this.close();
    });
  }

  searchUsers(searchUser: string): void {
    if (searchUser != '') {
      this.searchText = '';
      this.searchedUser = undefined;
      this.chatService.findUser(searchUser).subscribe((res) => {
        this.searchedUser = res;
      });
    }
  }

  close(): void {
    this.closeSearchModal.emit(this.addedUser);
    this.addedUser = false;
  }

  clearSearch(): void {
    this.searchText = '';
  }

}
