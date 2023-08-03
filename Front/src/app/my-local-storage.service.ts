import { Token, TokenContent } from './interfaces/users-interfaces';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { UserService } from './services/user.service';

@Injectable({
  providedIn: 'root'
})
export class MyLocalStorageService {

  constructor(private jwtHelper: JwtHelperService,private userService:UserService) { }
  checkToken(){
    const token = localStorage.getItem('userToken');
    if(token!==null){
      if(this.jwtHelper.isTokenExpired(token)){
         let userId:number=Number(localStorage.getItem('userId'));
        if(userId!==null){
        
          this.userService.refreshToken(userId).subscribe(
            {next:(res)=>{
           
            this.removeStorage();
            this.setStorage(res);
            return true;
            },        
            error:(err)=>{
            return false;
        }
      });
      }}
      return true;
    }
    return false;
  }
  removeStorage(){
    localStorage.clear();
  }
   setStorage(token:Token){
      localStorage.setItem('userToken',token.tokenContent);
      const tokenData=this.jwtHelper.decodeToken(token.tokenContent) as TokenContent;
      localStorage.setItem('userId',tokenData.userId.toString());
   }
}
