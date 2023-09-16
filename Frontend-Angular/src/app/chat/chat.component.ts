import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ChatService } from '../services/chat.service';
import { Chat } from '../interfaces/chat-interfaces';
import { FileMessage, Message, TextMessage } from '../interfaces/message-interfaces';
import { MessageService } from '../services/message.service';
import { HttpClient } from '@angular/common/http';
import { UserService } from '../services/user.service';
import { User, UserRole } from '../interfaces/users-interfaces';
import { forkJoin } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { ChooseAdminModalComponent } from '../modals/choose-admin-modal/choose-admin-modal.component';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})

export class ChatComponent implements OnInit {
  
  constructor(private route:ActivatedRoute, private chatService:ChatService, private userService:UserService,
              private messageService:MessageService, private router:Router, private dialog: MatDialog) 
  { }

  chatId?: number;
  chat?: Chat;
  chatSenders?: User[] = [];
  chatMembers?: User[] = [];

  userId?: number;
  userRole?: UserRole;
  userRoles: UserRole[] = [];

  messagesPageNumber: number = 1;
  usersPageNumber: number = 1;
  messagesPages: number[] = [];
  userPages: number[] = [];

  typeTextMessage?: TextMessage;
  typeFileMessage?: FileMessage;
  textMessages?: TextMessage[] = [];
  fileMesssages: FileMessage[] = [];
  messages: Message[] = [];

  showSearchModal = false;
  showImageModal = false;

  mediaViewerModalUrl = '';
  mediaViewerModalName = '';
  mediaViewerModalType = '';

  messageText = '';
  fileName = '';

  ngOnInit(): void {
    this.userId = Number(localStorage.getItem('userId'));
    this.route.paramMap.subscribe(params => {
      this.chatId = Number(params.get('id')!);
    });

    this.loadMessages(this.messagesPageNumber);

    this.setNumberOfMessagesPages();
    this.setNumberOfUsersPages();
  }

  getFileType(fileName: string): string {
    const extension = fileName.split('.').pop()?.toLowerCase();
    switch (extension) {
      case 'jpg':
      case 'jpeg':
      case 'png':
      case 'gif':
        return 'image';
      case 'mp4':
      case 'webm':
      case 'ogg':
        return 'video';
      case 'mp3':
      case 'wav':
        return 'audio';
      case 'pdf':
        return 'pdf';
      case 'doc':
      case 'docx':
        return 'word';
      case 'xls':
      case 'xlsx':
        return 'excel';
      case 'ppt':
      case 'pptx':
        return 'powerpoint';
      default:
        return 'other';
    }
  }

  getSenderName(senderId: number): string {
    return this.chatSenders?.find(user => user.id === senderId)?.username!;
  }

  isAdmin(userId : number): boolean {
    return this.userRoles.find(user => user.id === userId)?.role === 'Admin';
  }

  loadMessages(pageNumber : number): void {
    this.messagesPageNumber = pageNumber;
    this.chatService.getFullChat(Number(this.chatId), this.messagesPageNumber).subscribe((chatRes) => {
      this.chat = chatRes;
      
      this.messages = [];
      this.chatSenders = [];

      this.loadChatUsers(this.usersPageNumber);
      this.chat?.allMessages?.forEach(message => {
        
        this.userService.get(message.senderId).subscribe((resUser) => {
          if(this.chatSenders?.find(user => user.id === resUser.id) === undefined)
          this.chatSenders?.push(resUser);
        });

        if(message.messageType == 'text')
        {
          this.messageService.getText(message.id).subscribe((resText) => {
            this.messages.push(resText);
            this.messages.sort((a, b) => new Date(b.timeStamp).getTime() - new Date(a.timeStamp).getTime());
          });
        }
        else if(message.messageType == 'file')
        {
          this.messageService.getFile(message.id).subscribe((resFile) => {
            this.messageService.file(this.chat!.name, resFile.name).subscribe((fileData:Blob) => {
              const fileUrl = URL.createObjectURL(fileData);
              const fileMessage: FileMessage = {path: fileUrl, name: resFile.name, fileType: fileData.type, id: message.id, chatId: message.chatId, senderId: message.senderId, timeStamp: message.timeStamp, messageType: message.messageType};
              this.messages.push(fileMessage);
              this.messages.sort((a, b) => new Date(b.timeStamp).getTime() - new Date(a.timeStamp).getTime());
            });
          });
        }
      });
    });
  }

  loadChatUsers(pageNumber:number): void {
    this.usersPageNumber = pageNumber;
    this.chatService.getUsersInChat(this.chatId!, this.usersPageNumber).subscribe((resUsers) => {
      this.chatMembers = resUsers;
      this.userRoles = [];

      this.chat?.users?.forEach(user => {
        this.chatService.getUserRole(this.chatId!, user.id).subscribe((resRole) => {
          this.userRoles.push({id: user.id, role: resRole});
        });
      });
    });
  }

  setNumberOfMessagesPages(): void {
    this.chatService.getChatMessagesCount(this.chatId!).subscribe((resCount) => {
      let howManyPages = Math.ceil(resCount / 5);
      this.messagesPages = [];
      for (let i = 1; i <= howManyPages; i++) {
        this.messagesPages?.push(i);
      }
    });
  }

  setNumberOfUsersPages(): void {
    this.chatService.getChatUsersCount(this.chatId!).subscribe((resCount) => {
      let howManyPages = Math.ceil(resCount / 5);
      this.userPages = [];
      for (let i = 1; i <= howManyPages; i++) {
        this.userPages?.push(i);
      }
    });
  }

  onSubmitMessage(event: Event): void {
    event.preventDefault();
  
    const observables = [];
  
    if (this.messageText) {
      const formData = new FormData();
      formData.append('content', this.messageText);
      formData.append('chatId', this.chatId!.toString());
      formData.append('senderId', this.userId!.toString());
      formData.append('timeStamp', new Date(new Date().getTime()).toISOString());

      observables.push(this.messageService.postText(formData));
    }
  
    if (this.fileName) {
      const fileInput = document.getElementById('file') as HTMLInputElement;
      const file = fileInput.files?.[0];
      if (file) {
        const formData = new FormData();
        formData.append('name', file.name);
        formData.append('file', file, file.name);
        formData.append('chatId', this.chatId!.toString());
        formData.append('senderId', this.userId!.toString());
        formData.append('timeStamp', new Date(new Date().getTime()).toISOString());

        observables.push(this.messageService.postFile(formData));
      }
    }
  
    if (observables.length > 0) {
      forkJoin(observables).subscribe(() => {
        console.log('Messages sent successfully');
        this.loadMessages(1);
        this.setNumberOfMessagesPages();
      });
    }
  
    this.messageText = '';
    this.fileName = '';
    const fileInput = document.getElementById('file') as HTMLInputElement;
    fileInput.value = '';
  }

  onFileSelected(event: Event): void {
    const fileInput = event.target as HTMLInputElement;
    const file = fileInput.files?.[0];
    if (file) {
      this.fileName = file.name;
    } else {
      this.fileName = '';
    }
  }

  onRemoveFile(): void {
    this.fileName = '';
    const fileInput = document.getElementById('file') as HTMLInputElement;
    fileInput.value = '';
  }

  removeMessage(messageId: number, messageType: string): void {
    if(window.confirm('Czy na pewno chcesz usunąć wiadomość?'))
    {
      if (messageType == 'text') {
        this.messageService.deleteText(messageId).subscribe(() => {
          console.log("Wiadomość została usunięta");
          this.loadMessages(this.messagesPageNumber);
          this.setNumberOfMessagesPages();
        });
      }
      else if (messageType == 'file') {
        this.messageService.deleteFile(messageId).subscribe(() => {
          console.log("Plik został usunięty");
          this.loadMessages(this.messagesPageNumber);
          this.setNumberOfMessagesPages();
        });
      }
    }
  }

  openSearchModal(): void {
    this.showSearchModal = true;
  }

  closeSearchModal(addedUser: boolean): void {
    this.showSearchModal = false;
    if (addedUser)
    {
      this.loadChatUsers(1);
      this.setNumberOfUsersPages();
    }
    
  }

  openMediaViewerModal(url: string, name: string, type: string) {
    this.showImageModal = true;
    this.mediaViewerModalUrl = url;
    this.mediaViewerModalName = name;
    this.mediaViewerModalType = type;
  }

  closeMediaViewerModal() {
    this.showImageModal = false;
    this.mediaViewerModalUrl = '';
    this.mediaViewerModalName = '';
    this.mediaViewerModalType = '';
  }

  leaveChat(): void {
    // if(window.confirm('Czy na pewno chcesz opuścić czat?'))
    // {
    //   this.chatService.deleteUser(this.chatId!, this.userId!).subscribe(()=>{
    //     console.log("Użytkownik został usunięty z czatu");
        
    //     this.router.navigateByUrl('/chats');
    //   });
    // }

    if (this.isAdmin(this.userId!) && this.chat?.users?.length! > 1) {
      const dialogRef = this.dialog.open(ChooseAdminModalComponent, {
        width: '400px',
        data: { chatId: this.chatId, currentAdminId: this.userId, users: this.chat?.users }
      });
  
      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.chatService.assignAdmin(this.chatId!, result.id).subscribe(() => {

            console.log("Admin został zmieniony");

            this.chatService.deleteUser(this.chatId!, this.userId!).subscribe(()=>{
              console.log("Opuszczono czat");
              this.router.navigateByUrl('/chats');
            });
          });
        }
      });
    }
    else {
      this.chatService.deleteUser(this.chatId!, this.userId!).subscribe(()=>{
        console.log("Użytkownik został usunięty z czatu");
        this.router.navigateByUrl('/chats');
      });
    }
  }

  deleteChat(): void {
    if(window.confirm('Czy na pewno chcesz usunąć czat?'))
    {
      this.chatService.delete(this.chatId!).subscribe(()=>{
        console.log("Czat został usunięty");
        
        this.router.navigateByUrl('/chats');
      });
    }
  }

}

