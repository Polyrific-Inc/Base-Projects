import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { map, } from 'rxjs/operators';
import { User } from './user';
import * as jwt_decode from 'jwt-decode';
import { AuthorizePolicy } from './authorize-policy';
import { Role } from './role';
import { Config, ConfigService } from '../../config/config.service';
import { UserManager, User as OidcUser } from 'oidc-client';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private config: Config;
  private currentUserSubject: BehaviorSubject<User>;
  private temporaryUser: User;
  public currentUser: Observable<User>;

  private manager: UserManager;
  private oidcUser: OidcUser | null;

  get isLoggedIn() {
    return this.currentUser.pipe(map((currentUser: User) => {
      return currentUser != null;
    }));
  }

  constructor(
    private configService: ConfigService,
    private http: HttpClient) {
      this.config = this.configService.getConfig();
      this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
      this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
      return this.currentUserSubject.value;
  }

  public get isAdmin() {
    return this.currentUserValue.role == Role.Administrator;
  }

  public get isEditor() {
    return this.currentUserValue.role == Role.Editor;
  }

  login(user: User) {
    if (!this.config) {
      this.config = this.configService.getConfig();
    }

    if (this.config.enableSso) {
      if (!this.manager) {
        this.manager = new UserManager(this.config.oidc);
      }

      this.manager.signinRedirect();
    } else {
      if (!user.userName && !user.password && this.temporaryUser) {
        user.userName = this.temporaryUser.userName;
        user.password = this.temporaryUser.password;
      }

      return this.http.post(`${this.config.apiUrl}/Token`, user,
      {
        responseType: 'text'
      }).pipe(map(this.storeToken(user)));
    }
  }

  refreshSession() {
    if (!this.config) {
      this.config = this.configService.getConfig();
    }

    return this.http.get(`${this.config.apiUrl}/Token/refresh`,
    {
      responseType: 'text'
    }).pipe(map(this.storeToken(this.currentUserValue)));
  }

  logout() {
    if (!this.config) {
      this.config = this.configService.getConfig();
    }

    if (this.config.enableSso) {
      if (!this.manager) {
        this.manager = new UserManager(this.config.oidc);
      }

      this.manager.signoutRedirect();
    }

    // remove user from local storage to log user out
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  checkRoleAuthorization(authPolicy: AuthorizePolicy): boolean {
    const currentUser = this.currentUserValue;
    if (currentUser.role === Role.Administrator) {
      return true;
    }

    switch (authPolicy) {
      case AuthorizePolicy.UserRoleAdminAccess:
        return currentUser.role === Role.Administrator;
      case AuthorizePolicy.UserRoleEditorAccess:
        return [Role.Administrator, Role.Editor].includes(Role[currentUser.role]);
      case AuthorizePolicy.UserRoleGuestAccess:
        return [Role.Administrator,Role.Editor, Role.Guest].includes(Role[currentUser.role]);
      default:
      return false;
     }
  }

  getCurrentUserPhotoUrl() {
    let userName = "unknown";
    if(this.currentUserValue){
        userName = this.currentUserValue.userName;
    }

    return `${this.configService.getConfig().apiUrl}/userprofile/${userName}/photo`
}

  async completeAuthentication() {
    if (!this.config) {
      this.config = this.configService.getConfig();
    }

    if (!this.manager) {
      this.manager = new UserManager(this.config.oidc);
    }

    this.oidcUser = await this.manager.signinRedirectCallback();

    this.storeOidcToken(this.oidcUser.access_token);
  }

  private storeToken(user: User) {
    return (token: string) => {
        if (token === 'Requires two factor') {
          // temporarily store user object for next process
          this.temporaryUser = user;
          return token;
        } else if (token) {
          // store user details and jwt token in local storage to keep user logged in between page refreshes
          user.token = token;
          const decodedToken = this.getDecodedAccessToken(token);

          if (decodedToken.hasOwnProperty('http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier')) {
            user.id = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
          }

          if (decodedToken.hasOwnProperty('http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name')) {
            user.userName = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
          }

          if (decodedToken.hasOwnProperty('http://schemas.microsoft.com/ws/2008/06/identity/claims/role')) {
            user.role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
          }

          if (decodedToken.hasOwnProperty('http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname')) {
            user.firstName = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname'];
          }

          if (decodedToken.hasOwnProperty('http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname')) {
            user.lastName = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname'];
          }

          user.password = null;
          user.tokenExpired = decodedToken.exp;
          this.temporaryUser = null;

          localStorage.setItem('currentUser', JSON.stringify(user));
          this.currentUserSubject.next(user);
      }

      return user;
    };
  }

  private storeOidcToken(token: string) {
    const decodedToken = this.getDecodedAccessToken(token);

    var user = <User>{
      token: token,
      tokenExpired: decodedToken.exp
    }

    if (decodedToken.hasOwnProperty('upn')) {
      user.userName = decodedToken['upn']
    }

    if (decodedToken.hasOwnProperty('given_name')) {
      user.firstName = decodedToken['given_name']
    } else if (decodedToken.hasOwnProperty('name')) {
      user.firstName = decodedToken['name']
    }

    if (decodedToken.hasOwnProperty('family_name')) {
      user.lastName = decodedToken['family_name']
    }

    if (decodedToken.hasOwnProperty('roles')) {
      user.role = Array.from(decodedToken['roles'])[0].toString();
    }

    localStorage.setItem('currentUser', JSON.stringify(user));
    this.currentUserSubject.next(user);

    return user;
  }

  private getDecodedAccessToken(token: string): any {
    try {
        return jwt_decode(token);
    } catch (Error) {
        return null;
    }
  }
}
