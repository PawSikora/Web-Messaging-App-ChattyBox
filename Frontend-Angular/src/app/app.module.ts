import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import{JwtModule} from '@auth0/angular-jwt';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { MainPageComponent } from './main-page/main-page.component';
import { ChatsMenuComponent } from './chats-menu/chats-menu.component';
import { ChatComponent } from './chat/chat.component';
import { SideMenuComponent } from './side-menu/side-menu.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatListModule } from '@angular/material/list';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { AsPipe } from './cast.pipe';
import { MediaViewerModalComponent } from './modals/media-viewer-modal/media-viewer-modal.component';
import { SearchUserModalComponent } from './modals/search-user-modal/search-user-modal.component';
import { ToastMessageComponent } from './toast-message/toast-message.component';
import { CreateChatModalComponent } from './modals/create-chat-modal/create-chat-modal.component';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    MainPageComponent,
    ChatsMenuComponent,
    ChatComponent,
    SideMenuComponent,
    AsPipe,
    MediaViewerModalComponent,
    SearchUserModalComponent,
    ToastMessageComponent,
    CreateChatModalComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: () =>  localStorage.getItem('userToken'),
        allowedDomains: ['localhost:5191'],
        disallowedRoutes: [] 
      }}),
    NgbModule,
    BrowserAnimationsModule,
    MatIconModule,
    MatButtonModule,
    MatSnackBarModule,
    MatListModule,
    MatCardModule,
    MatFormFieldModule,
    MatGridListModule,

  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
