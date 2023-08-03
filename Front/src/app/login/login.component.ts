import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { Token } from '../interfaces/users-interfaces';
import { MyLocalStorageService } from '../my-local-storage.service';
import { EMAIL_PATTERN} from '../constants/validation-patterns.const';
import { ToastMessageService } from '../services/toast-message.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements  OnInit {
  registerMode=false;
  constructor(private myLocalStorage:MyLocalStorageService,private formBuilder: FormBuilder,
      private router: Router,private form:FormBuilder,private userService:UserService,
      private toastMessageService:ToastMessageService) {}
  loginForm!: FormGroup;
  registerForm!: FormGroup;

  ngOnInit(): void {
    this.loginForm= this.formBuilder.group({
      email: [null,Validators.compose([Validators.required,Validators.pattern(EMAIL_PATTERN)])],
      password: [null,Validators.compose([Validators.required])]
      });
    this.registerForm= this.formBuilder.group({ 
    email: [null,Validators.compose([Validators.required,Validators.pattern(EMAIL_PATTERN)])],
    password: [null,Validators.compose([Validators.required])],
    confirmedPassword: [null,Validators.compose([Validators.required])],
    name: [null,Validators.compose([Validators.required,Validators.minLength(3),Validators.maxLength(32)])]
    });
  }
 onSubmitLogin() {
  console.log(this.loginForm.value);
  if(this.loginForm.valid){
    this.userService.login(this.loginForm.value).subscribe((res)=>{
        this.myLocalStorage.setStorage(res);
        this.refresh();
    });
  }
 }
 onSubmitRegister() {
  if(this.registerForm.valid){
    this.userService.register(this.registerForm.value).subscribe((res)=>{
      this.toastMessageService.notifyOfSuccess('Rejestracja przebiegła pomyślnie. Zaloguj się.');
      this.change();
    });
  }
 }
 private refresh(){
  setTimeout(() => this.router.navigate(['main-page']), 100);

 }

 change(){
  this.registerMode=!this.registerMode;
  this.registerForm.reset();
  this.loginForm.reset();
 }
}

