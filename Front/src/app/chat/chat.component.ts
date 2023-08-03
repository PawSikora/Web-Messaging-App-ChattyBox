import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ChatService } from '../services/chat.service';
import { Chat } from '../interfaces/chat-interfaces';
import { FileMessage, Message, TextMessage } from '../interfaces/message-interfaces';
import { FileService } from '../services/file.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
  constructor(private route:ActivatedRoute,private chatSerivice:ChatService,
    private fileService:FileService,private http:HttpClient ) {
    }
  private id?: string;
  chat?:Chat
  textMessages?:TextMessage[]=[];
  files: { name: string, url: string, type: string }[] = [];

  
  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.id = params.get('id')!;
      this.chatSerivice.getFullChat(Number(this.id),1).subscribe((chatRes)=>{
        this.chat=chatRes;
          this.chat!.allMessages!.forEach(element => {
            if(element.messageType=="file"){   
              this.fileService.getFile(element.id).subscribe((resFile)=>{
                this.fileService.file(this.chat!.name,resFile.name).subscribe((fileData:Blob)=>{
                  const fileUrl = URL.createObjectURL(fileData);
                  const fileType=fileData.type;
                  this.files.push({name:resFile.name,url:fileUrl,type:fileType});
                });
              });
            }
            else{
              this.fileService.getText(element.id).subscribe((resText)=>{
                this.textMessages!.push(resText);
              });
            }
          });
      });
  });
  }
isImage(path: string): boolean {
  const imageExtensions = ['jpg', 'jpeg', 'png', 'gif'];
  const extension = this.getFileExtension(path);
  return imageExtensions.includes(extension.toLowerCase());
}

isGif(path: string): boolean {
  const gifExtension = 'gif';
  const extension = this.getFileExtension(path);
  return extension.toLowerCase() === gifExtension;
}

isVideo(path: string): boolean {
  const videoExtensions = ['mp4', 'avi', 'mkv', 'mov'];
  const extension = this.getFileExtension(path);
  return videoExtensions.includes(extension.toLowerCase());
}

isAudio(path: string): boolean {
  const audioExtensions = ['mp3', 'wav', 'ogg'];
  const extension = this.getFileExtension(path);
  return audioExtensions.includes(extension.toLowerCase());
}

isOtherFile(path: string): boolean {
  return !this.isImage(path) && !this.isGif(path) && !this.isVideo(path) && !this.isAudio(path);
}

private getFileExtension(filename: string): string {
  return filename.split('.').pop() || '';
}


}

