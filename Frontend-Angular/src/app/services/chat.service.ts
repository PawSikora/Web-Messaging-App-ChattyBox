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

  constructor(private httpClient:HttpClient, private toastMessageService:ToastMessageService) 
  { }

  getFullChat(chatId:number,pageNumber:number):Observable<Chat> {
    const params = new HttpParams()
    .set('chatId',chatId.toString())
    .set('pageNumber',pageNumber.toString());

    return this.httpClient
    .get<Chat>(`${environment.httpBackend}${Api.CHAT}`,{params})
    .pipe(catchError((err:HttpErrorResponse) => {
      this.toastMessageService.notifyOfError(err.error);
      return of();
    }));
  }

  create(newChat: FormData):Observable<any> {

    return this.httpClient
    .post<any>(`${environment.httpBackend}${Api.CHAT_CREATE}`, newChat)
    .pipe(catchError((err:HttpErrorResponse) => {
      this.toastMessageService.notifyOfError(err.error);
      return of();
    }));
  }

  delete(id:number):Observable<Chat> {

    return this.httpClient
    .delete<Chat>(`${environment.httpBackend}${Api.CHAT_DELETE_CHAT}`
    .replace(':id', id.toString()))
    .pipe(catchError((err:HttpErrorResponse) => {
      this.toastMessageService.notifyOfError(err.error);
      return of();
    }));
  }


  addUser(chatId:number, userId:number):Observable<Chat> {
    
    return this.httpClient
    .put<Chat>(`${environment.httpBackend}${Api.CHAT_ADD_USER}`, {chatId, userId})
    .pipe(catchError((err:HttpErrorResponse) => {
      this.toastMessageService.notifyOfError(err.error);
      return of();
    }));
  }

  findUser(email:string):Observable<User> {
    const params = new HttpParams()
    .set('email',email);

    return this.httpClient
    .get<User>(`${environment.httpBackend}${Api.CHAT_FIND_USER}`, {params})
    .pipe(catchError((err:HttpErrorResponse) => {
      this.toastMessageService.notifyOfError(err.error);
      return of();
    }));
  }

  deleteUser(chatId:number,userId:number):Observable<Chat> {

    return this.httpClient
    .put<Chat>(`${environment.httpBackend}${Api.CHAT_DELETE_USER}`, {chatId, userId})
    .pipe(catchError((err:HttpErrorResponse) => {
      this.toastMessageService.notifyOfError(err.error);
      return of();
    }));
  }

  getUsersInChat(chatId:number, pageNumber:number):Observable<User[]> {
    const params = new HttpParams()
    .set('id', chatId.toString())
    .set('pageNumber', pageNumber.toString());

    return this.httpClient
    .get<User[]>(`${environment.httpBackend}${Api.CHAT_GET_USERS}`,{params})
    .pipe(catchError((err:HttpErrorResponse) => {
      this.toastMessageService.notifyOfError(err.error);
      return of();
    }));
  }

  getChatMessagesCount(chatId:number):Observable<number> {
    const params = new HttpParams()
    .set('id', chatId.toString());

    return this.httpClient
    .get<number>(`${environment.httpBackend}${Api.CHAT_GET_MESSAGES_COUNT}`,{params})
    .pipe(catchError((err:HttpErrorResponse) => {
      this.toastMessageService.notifyOfError(err.error);
      return of();
    }));
  }

  getChatUsersCount(chatId:number):Observable<number> {
    const params = new HttpParams()
    .set('id', chatId.toString());

    return this.httpClient
    .get<number>(`${environment.httpBackend}${Api.CHAT_GET_USERS_COUNT}`,{params})
    .pipe(catchError((err:HttpErrorResponse) => {
      this.toastMessageService.notifyOfError(err.error);
      return of();
    }));
  }

  assignAdmin(chatId: number, userId: number): Observable<any> {
    const params = new HttpParams()
      .set('chatId', chatId.toString())
      .set('userId', userId.toString());
  
    return this.httpClient
      .put<any>(`${environment.httpBackend}${Api.CHAT_ASSIGN_ADMIN}`, null, { params })
      .pipe(catchError((err: HttpErrorResponse) => {
        this.toastMessageService.notifyOfError(err.error);
        return of();
      }));
  }

  getUserRole(chatId:number, userId:number):Observable<string> {
    const params = new HttpParams()
    .set('chatId', chatId.toString())
    .set('userId', userId.toString());

    return this.httpClient
    .get<string>(`${environment.httpBackend}${Api.CHAT_GET_USER_ROLE}`,{params, responseType:'text' as 'json'})
    .pipe(catchError((err:HttpErrorResponse) => {
      this.toastMessageService.notifyOfError(err.error);
      return of();
    }))
  }

}
