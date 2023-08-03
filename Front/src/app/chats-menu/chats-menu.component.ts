import { Component, OnInit } from '@angular/core';
import { Chat } from '../interfaces/chat-interfaces';
import { UserService } from '../services/user.service';
import { UserChats } from '../interfaces/users-interfaces';
import { Router } from '@angular/router';

@Component({
  selector: 'app-chats',
  templateUrl: './chats-menu.component.html',
  styles: []
})
export class ChatsMenuComponent implements OnInit{
  
    constructor(private userService:UserService,private router:Router) { }
    chats:UserChats[]=[];
    private numberOfChats?:number;
    pageNumber?:number;
    userId?:number;
    pages?:number[]=[];

    ngOnInit(): void {
      this.downloadNumberOfChats();
      this.pageNumber = 1;
      this.userId = Number(localStorage.getItem('userId'));
      this.loadPage(this.pageNumber,this.userId);
    }

    downloadNumberOfChats():void{
      let userId:number = Number(localStorage.getItem('userId'));
      this.userService.get(userId).subscribe((res)=>{
        this.numberOfChats=res.chatsCount;
        this.setNumberOfPages();
      });
    }

    loadPage(pageNumber:number,userId:number):void{
      this.pageNumber = pageNumber;
      this.userService.getUserChats(userId,pageNumber).subscribe((res)=>{
        this.chats=res;
      });
    }
    setNumberOfPages():void{
      let howManyPages = Math.ceil(this.numberOfChats!/5);
      for(let i=1;i<=howManyPages;i++){
        this.pages?.push(i);
        
      }
    }
    openChat(chatId:number):void{
      this.router.navigateByUrl(`/chat/${chatId}`);
    }





}
