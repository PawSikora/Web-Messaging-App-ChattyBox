import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: `app.component.html`,
  styleUrls: ['app.component.css']
})
export class AppComponent {
  title = 'Angular';
  showHeader=true;
  constructor(private router:Router){
    router.events.subscribe((val) => {
      this.showHeader = !(this.router.url === '/login');
    });
  }
}
