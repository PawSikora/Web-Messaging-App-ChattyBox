import { Component, OnInit } from '@angular/core';
import { Chat } from '../interfaces/chat-interfaces';
import { UserService } from '../services/user.service';
import { User, UserChats, UserRole } from '../interfaces/users-interfaces';
import { Router } from '@angular/router';
import { ChatService } from '../services/chat.service';

@Component({
  selector: 'app-chats',
  templateUrl: './chats-menu.component.html',
  styleUrls: ['./chats-menu.component.css']
})

export class ChatsMenuComponent implements OnInit{
  
    constructor(private chatService:ChatService, private userService:UserService, private router:Router) 
    { }

    chatId!: number;
    userId?: number;

    chats: UserChats[] = [];
    chatUsers: User[] = [];
    usersRoles: UserRole[] = []; 
    searchedUser?: User;

    chatsPageNumber?: number;
    usersPageNumber?: number;
    chatPages: number[] = [];
    userPages: number[] = [];
    
    searchText: string = '';
    showSearchModal: boolean = false;
    showCreateChatModal: boolean = false;


    ngOnInit(): void {
      this.userId = Number(localStorage.getItem('userId'));
      this.chatsPageNumber = 1;
      this.usersPageNumber = 1;
      this.setNumberOfChatPages();
      this.loadPage(this.userId, this.chatsPageNumber!);
    }

    setNumberOfChatPages(): void {
      if(this.userId)
      this.userService.get(this.userId).subscribe((res) => {
        let numberOfChats = res.chatsCount;
        let howManyPages = Math.ceil(numberOfChats / 5);
        this.chatPages = [];
        for (let i = 1; i <= howManyPages; i++) {
          this.chatPages?.push(i);
        }
      });
    }
    
    setNumberOfUserPages(chatId: number): void {
      this.chatService.getFullChat(chatId, 1).subscribe((res) => {
        let numberOfUsers = res.users.length;
        let howManyPages = Math.ceil(numberOfUsers / 5);
        this.userPages = [];
        for (let i = 1; i <= howManyPages; i++) {
          this.userPages?.push(i);
        }
      });
    }

    loadPage(userId:number, pageNumber:number): void {
      this.chatsPageNumber = pageNumber;
      this.userService.getUserChats(userId, pageNumber).subscribe((res) => {
        this.chats = res;
        this.setNumberOfChatPages();
      });
    }

    loadChatUsers(chatId:number, pageNumber:number): void {
      this.chatId = chatId;
      this.usersPageNumber = pageNumber;
      this.chatService.getUsersInChat(chatId, pageNumber).subscribe((res) => {
        
        this.chatUsers = res;
        this.setNumberOfUserPages(chatId);
        this.usersRoles = [];

        this.chatService.getFullChat(chatId, 1).subscribe((res) => {

          
          res.users.forEach((user) => {
            this.chatService.getUserRole(chatId, user.id).subscribe((res) => {
              this.usersRoles.push({id: user.id, role: res});
            });
          });

        });


        
      });
    }

    removeUserFromChat(userId:number): void {
      if(window.confirm('Czy na pewno chcesz usunąć tego użytkownika z czatu?')) {
        this.chatService.deleteUser(this.chatId, userId).subscribe(() => {
        this.loadChatUsers(this.chatId!, 1);
      });
      }
    }

    isAdmin(userId:number): boolean {
      return this.usersRoles.find(user => user.id === userId)?.role === 'Admin';
    }

    openChat(chatId:number): void {
      this.router.navigateByUrl(`/chat/${chatId}`);
    }

    openSearchModal(): void {
      this.showSearchModal = true;
    }
  
    closeSearchModal(): void {
      this.showSearchModal = false;
      this.loadChatUsers(this.chatId!, 1);
    }

    openCreateChatModal(): void {
      this.showCreateChatModal = true;
    }

    closeCreateChatModal(): void {
      this.showCreateChatModal = false;
      this.loadPage(this.userId!, 1);
    }
}
