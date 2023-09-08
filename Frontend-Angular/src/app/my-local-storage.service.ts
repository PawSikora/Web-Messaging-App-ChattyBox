import { Token, TokenContent } from './interfaces/users-interfaces';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { UserService } from './services/user.service';

@Injectable({
  providedIn: 'root'
})
export class MyLocalStorageService {

  constructor(private jwtHelper: JwtHelperService,private userService:UserService) 
  { }
  
  checkToken(): boolean {
    const token = localStorage.getItem('userToken');
  
    if (!token) {
      return false;
    }
  
    if (!this.jwtHelper.isTokenExpired(token)) {
      return true;
    }
  
    const userId = Number(localStorage.getItem('userId'));
  
    if (!userId) {
      return false;
    }
  
    this.userService.refreshToken(userId).subscribe(res => {
      this.removeStorage();
      this.setStorage(res);
    });
  
    return true;
  }

  removeStorage() {
    localStorage.clear();
  }

   setStorage(token:Token){
      localStorage.setItem('userToken',token.tokenContent);
      const tokenData=this.jwtHelper.decodeToken(token.tokenContent) as TokenContent;
      localStorage.setItem('userId',tokenData.userId.toString());
   }
}
