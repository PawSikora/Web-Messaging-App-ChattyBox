import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { MyLocalStorageService } from '../my-local-storage.service';
import { UserService } from '../services/user.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private router:Router,private mylocalstorage:MyLocalStorageService,private userService:UserService) { }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      if(localStorage.getItem("userToken")!==null && this.mylocalstorage.checkToken()){
        return true;
      }
      this.router.navigate(['/login']);
      console.log(this.mylocalstorage.checkToken());
      return false;
      }
  
}
