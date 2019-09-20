import { Component, OnInit } from '@angular/core';
import { AuthService } from '@app/core/auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  isExpanded = false;
  isLoggedIn = false;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit() {
    this.authService.currentUser.subscribe(user => {
      if (user) {
        this.isLoggedIn = true;
      } else {
        this.isLoggedIn = false;
      }
    })
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  onLogout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
