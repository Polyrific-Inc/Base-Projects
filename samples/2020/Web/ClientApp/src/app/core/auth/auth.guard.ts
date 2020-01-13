import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, CanActivateChild } from '@angular/router';
import { Config, ConfigService } from '@app/config/config.service';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate, CanActivateChild {
  private config: Config;

  constructor(
    private authService: AuthService,
    private router: Router,
    private configService: ConfigService
  ) {
    this.config = this.configService.getConfig();
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot) {
      const currentUser = this.authService.currentUserValue;
      if (currentUser) {
        // logout if token expired
        if (Date.now() >= currentUser.tokenExpired * 1000) {
          this.authService.logout();
          this.router.navigate(['/login'], { queryParams: { returnUrl: state.url }});
        }

        // check if route is restricted by role
        if (route.data.authPolicy != null &&
          !this.authService.checkRoleAuthorization(route.data.authPolicy)) {
            // role not authorized so redirect to unauthorized error page
            this.router.navigate(['/unauthorized']);
            return false;
        }

        // authorized so return true
        return true;
      }

      // not logged in so redirect to login page with the return url
      this.router.navigate(['/login'], { queryParams: { returnUrl: state.url }});
      return false;
    }

    canActivateChild (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
      return this.canActivate(route, state);
    }
}
