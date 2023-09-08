import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ChatService } from 'src/app/services/chat.service';

@Component({
  selector: 'app-create-chat-modal',
  templateUrl: './create-chat-modal.component.html',
  styleUrls: ['./create-chat-modal.component.css']
})
export class CreateChatModalComponent {

  constructor(private chatService:ChatService) { }

  @Input() userId!: number;
  @Output() closeCreateChatModal = new EventEmitter<void>();

  chatName: string = '';

  createChat() {
    if(this.chatName)
    {
      const formData = new FormData();
      formData.append('UserId', this.userId.toString());
      formData.append('Name', this.chatName);

      this.chatService.create(formData).subscribe(() => {
        console.log('Chat created ' + this.chatName + ' ' + this.userId);
        this.closeCreateChat();
      });
    }
  }


  closeCreateChat() {
    this.closeCreateChatModal.emit();
  }
}
