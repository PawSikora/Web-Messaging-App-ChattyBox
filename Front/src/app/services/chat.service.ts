import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Chat } from '../interfaces/chat-interfaces';
import { Observable, catchError, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Api } from '../enums/api.enum';
import { User } from '../interfaces/users-interfaces';
import { ToastMessageService } from './toast-message.service';
@Injectable({
  providedIn: 'root'
})
export class ChatService {

  constructor(private httpClient:HttpClient,private toastMessageService:ToastMessageService) { }
  get(id:number):Observable<Chat>{
    return this.httpClient      
    .get<Chat>(`${environment.httpBackend}${Api.CHAT_GET}`
    .replace(':id', id.toString()))
    .pipe(catchError((err:HttpErrorResponse)=>{
      const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
      return of ();
    })); 
  }
  delete(id:number):Observable<Chat>{
    return this.httpClient
    .delete<Chat>(`${environment.httpBackend}${Api.CHAT_DELETE_CHAT}`
    .replace(':id', id.toString()))
    .pipe(catchError((err:HttpErrorResponse)=>{
      const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);;
      return of ();
    }));
  }
  create(chat:Chat):Observable<any>{

    return this.httpClient
    .post<any>(`${environment.httpBackend}${Api.CHAT_CREATE}`,chat)
    .pipe(catchError((err:HttpErrorResponse)=>{
      const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
      return of ();
    }));
  }
  addUser(chatId:number,userId:number):Observable<Chat>{
    const params = new HttpParams()
    .set('chatId',chatId.toString())
    .set('userId',userId.toString());
    
    return this.httpClient
    .put<Chat>(`${environment.httpBackend}${Api.CHAT_ADD_USER}`,{params})
    .pipe(catchError((err:HttpErrorResponse)=>{
      const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
      return of ();
    }));
  }
  findUser(email:string):Observable<User>{
    const params = new HttpParams()
    .set('email',email);

    return this.httpClient
    .get<User>(`${environment.httpBackend}${Api.CHAT_FIND_USER}`,{params})
    .pipe(catchError((err:HttpErrorResponse)=>{
      const error = err.error as string[];
      this.toastMessageService.notifyOfError(error[0]);
      return of ();
    }));
  }

  getFullChat(chatId:number,pageNumber:number):Observable<Chat>{
    const params = new HttpParams()
    .set('chatId',chatId.toString())
    .set('pageNumber',pageNumber.toString());
    return this.httpClient
    .get<Chat>(`${environment.httpBackend}${Api.CHAT}`,{params})
    .pipe(catchError((err:HttpErrorResponse)=>{
      const error = err.error as string[];
  
      this.toastMessageService.notifyOfError(error[0]);
      return of ();
    }));
  }




}
