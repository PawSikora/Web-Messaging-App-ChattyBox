import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, of } from 'rxjs';
import { FileMessage, GetNewsestMessagesText, TextMessage } from '../interfaces/message-interfaces';
import { Api } from '../enums/api.enum';
import { environment } from 'src/environments/environment';
import { ToastMessageService } from './toast-message.service';

@Injectable({
  providedIn: 'root'
})
export class FileService {

  constructor(private httpClient:HttpClient,private toastMessageService:ToastMessageService) { }

  getFile(id:number):Observable<FileMessage>{

    return this.httpClient.
    get<FileMessage>(`${environment.httpBackend}${Api.FILEMESSAGE}`
    .replace(':id', id.toString()))
    .pipe(catchError((err:HttpErrorResponse)=>{
      const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
      return of();}))
  }
  deleteFile(id:number):Observable<FileMessage>{

    return this.httpClient.
    delete<FileMessage>(`${environment.httpBackend}${Api.FILEMESSAGE_DELETE}`
    .replace(':id', id.toString()))
    .pipe(catchError((err:HttpErrorResponse)=>{
      const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
      return of();}))
  }
  postFile(file:FileMessage):Observable<any>{

    return this.httpClient.
    post<any>(`${environment.httpBackend}${Api.FILEMESSAGE_POST}`,file)
    .pipe(catchError((err:HttpErrorResponse)=>{
      const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
      return of();}))
  }
  getNewsest(id:number):Observable<GetNewsestMessagesText>{

    return this.httpClient.
    get<GetNewsestMessagesText>(`${environment.httpBackend}${Api.FILEMESSAGE_GET}`
    .replace(':id', id.toString()))
    .pipe(catchError((err:HttpErrorResponse)=>{
      const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
      return of();}))
  }
  getText(id:number):Observable<TextMessage>{

    return this.httpClient.
    get<TextMessage>(`${environment.httpBackend}${Api.TEXTMESSAGE}`
    .replace(':id', id.toString()))
    .pipe(catchError((err:HttpErrorResponse)=>{
      const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
      return of();}))
  }
  postText(text:TextMessage):Observable<any>{

    return this.httpClient.
    post<any>(`${environment.httpBackend}${Api.TEXTMESSAGE_POST}`,text)
    .pipe(catchError((err:HttpErrorResponse)=>{
      const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
      return of();}))
  }
  deleteText(id:number):Observable<TextMessage>{
    
    return this.httpClient.
    delete<TextMessage>(`${environment.httpBackend}${Api.TEXTMESSAGE_DELETE}`
    .replace(':id', id.toString()))
    .pipe(catchError((err:HttpErrorResponse)=>{
      const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
      return of();}))
  }
  
  file(chatname:string,fileName:string):Observable<Blob>{
    const url = `${environment.httpBackend}${Api.FILE}?chatname=${chatname}&fileName=${fileName}`;
    const headers = new HttpHeaders().set('Accept', 'application/octet-stream');
    return this.httpClient.get(url, { responseType: 'blob', headers: headers }).pipe(
      catchError((err: HttpErrorResponse) => {
        console.log('Wystąpił błąd podczas pobierania pliku:', err);
        return of();
      })
    );
  }
}

