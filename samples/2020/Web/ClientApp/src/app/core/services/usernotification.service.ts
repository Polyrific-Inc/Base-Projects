///OpenCatapultModelId:942
import { Injectable } from '@angular/core';
import { UserNotificationDto } from '../models/usernotification/usernotification-dto';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class UserNotificationService {

  constructor(private apiService: ApiService) { }

  createUserNotification(dto: UserNotificationDto) {
    return this.apiService.post('UserNotification', dto);
  }
}

