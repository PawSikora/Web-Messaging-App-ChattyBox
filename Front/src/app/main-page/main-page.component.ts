import { Component, Input, OnInit } from '@angular/core';
import { MyLocalStorageService } from '../my-local-storage.service';
import { UserService } from '../services/user.service';
import { User } from '../interfaces/users-interfaces';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: []
})
export class MainPageComponent implements OnInit{
  constructor(private myLocalStorage:MyLocalStorageService,private userService:UserService) { }
  user?:User;
  userId?:number;
  ngOnInit(): void {
    this.userId =Number(localStorage.getItem('userId'));
    this.userService.get(this.userId).subscribe((res)=>{
      this.user=res;
      console.log(this.user?.chatsCount);
    });
  }
}
