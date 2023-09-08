import { Component } from '@angular/core';
import { MyLocalStorageService } from '../my-local-storage.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-side-menu',
  templateUrl: './side-menu.component.html',
  styleUrls: ['./side-menu.component.css']
})
export class SideMenuComponent {
  constructor(private myLocalStorageService:MyLocalStorageService,private router:Router) { }

  logout(){
    this.myLocalStorageService.removeStorage();
    this.router.navigate(['/login']);
  }
}
