import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { map, } from 'rxjs/operators';
import { User } from './user';
import * as jwt_decode from 'jwt-decode';
import { AuthorizePolicy } from './authorize-policy';
import { Role } from './role';
import { Config, ConfigService } from '../../config/config.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private config: Config;
  private currentUserSubject: BehaviorSubject<User>;
  private temporaryUser: User;
  public currentUser: Observable<User>;
  
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

    if (!user.userName && !user.password && this.temporaryUser) {
      user.userName = this.temporaryUser.userName;
      user.password = this.temporaryUser.password;
    }

    // TODO: Delete this in real app
    if (this.config.bypassLogin) {    
      user.id = 1;
      user.userName = 'DummyUser';
      user.role = Role.Administrator;
      user.firstName = "Dummy";
      user.lastName = "User";
      user.password = null;
      user.tokenExpired = 99999999999;
      this.temporaryUser = null;

      localStorage.setItem('currentUser', JSON.stringify(user));
      this.currentUserSubject.next(user);
      return of(user);
    }

    return this.http.post(`${this.config.apiUrl}/Token`, user,
      {
        responseType: 'text'
      }).pipe(map(this.storeToken(user)));
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
  
  private storeToken(user: User) {
    return (token: string) => {
        if (token) {
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

  private getDecodedAccessToken(token: string): any {
    try {
        return jwt_decode(token);
    } catch (Error) {
        return null;
    }
  }
}
