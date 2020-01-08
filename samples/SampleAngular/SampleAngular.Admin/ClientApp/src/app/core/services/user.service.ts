import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { UserDto } from '../models/user/user-dto';
import { ResetPasswordDto } from '../models/user/reset-password-dto';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private apiService: ApiService) { }

  getUsers(): Observable<UserDto[]> {
    return this.apiService.get<UserDto[]>('user');
  }

  getUser(userId: number): Observable<UserDto> {
    return this.apiService.get<UserDto>(`user/${userId}`);
  }

  createUser(user: UserDto) {
    return this.apiService.post('user', user);
  }

  updateUser(user: UserDto) {
    return this.apiService.put(`user/${user.id}`, user);
  }

  deleteUser(userId: number) {
    return this.apiService.delete(`user/${userId}`);
  }

  requestResetPassword(username: string) {
    return this.apiService.getString(`user/name/${username}/resetpassword`);
  }

  resetPassword(username: string, dto: ResetPasswordDto) {
    return this.apiService.postString(`user/name/${username}/resetpassword`, dto);
  }

  confirmEmail(userId: number, token: string) {
    return this.apiService.getString(`user/${userId}/confirm?token=${token}`);
  }
}
