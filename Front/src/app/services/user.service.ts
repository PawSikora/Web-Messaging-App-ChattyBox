import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Observable, catchError, of } from 'rxjs';
import { Login, Register, User,Token, TokenContent, UserChats } from '../interfaces/users-interfaces';
import { environment } from 'src/environments/environment';
import { Api } from '../enums/api.enum';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ToastMessageService } from './toast-message.service';


@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private httpClient:HttpClient,private jwtHelper:JwtHelperService,private toastMessageService:ToastMessageService) { }
  get(id:number):Observable<User>{
    return this.httpClient      
    .get<User>(`${environment.httpBackend}${Api.USER}`
    .replace(':id', id.toString()))
    .pipe(catchError((err:HttpErrorResponse)=>{
      const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
      return of ();
    })); 
  }
  register(register:Register):Observable<any>{
    console.log(register);
    return this.httpClient
    .post<any>(`${environment.httpBackend}${Api.REGISTER}`,register)
    .pipe(catchError((err:HttpErrorResponse)=>{
      const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
      return of ();
    }));
  }
  refreshToken(userId:number):Observable<Token>{
    const options = {
      params: { 'userId': userId },withCredentials:true
    };
    
    return this.httpClient
    .post<Token>(`${environment.httpBackend}${Api.REFRESH_TOKEN}`,null,options)
    .pipe(catchError((err:HttpErrorResponse)=>{
      const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
      return of ();
    }));

  }

  login(login:Login):Observable<Token>{

    return this.httpClient
    .post<Token>(`${environment.httpBackend}${Api.LOGIN}`,login,{withCredentials:true})
    .pipe(catchError((err:HttpErrorResponse)=>{
      const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
      return of ();
    }));
  }
  getUserChats(chatId:number,pageNumber:number):Observable<UserChats[]>{
    const params = new HttpParams()
    .set('id',chatId.toString())
    .set('pageNumber',pageNumber.toString());
    
    return this.httpClient
    .get<UserChats[]>(`${environment.httpBackend}${Api.USER_CHATS}`, {params})
    .pipe(catchError((err:HttpErrorResponse)=>{
      const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
      return of ();
    }));
  }

}
