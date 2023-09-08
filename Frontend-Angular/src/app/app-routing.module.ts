import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { main } from '@popperjs/core';
import { MainPageComponent } from './main-page/main-page.component';
import { ChatsMenuComponent } from './chats-menu/chats-menu.component';
import { ChatComponent } from './chat/chat.component';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  {path:'login',component:LoginComponent },
  {path: '',redirectTo: '/main-page',pathMatch: 'full' },
  {path:'main-page',canActivate:[AuthGuard],component:MainPageComponent },
  {path: 'chats',canActivate:[AuthGuard], component: ChatsMenuComponent },
  {path: 'chat',canActivate:[AuthGuard],
  children:[{path:':id',component:ChatComponent}]
  },
  ];
;

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
