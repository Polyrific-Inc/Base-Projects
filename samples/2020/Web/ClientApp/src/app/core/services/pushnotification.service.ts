import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PushNotificationDto } from '../models/pushnotification/pushnotification-dto';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class PushNotificationService {

  constructor(private apiService: ApiService) { }

    PushNotification(dto: PushNotificationDto) {
        return this.apiService.post('PushNotification/NotifyAllDevices', dto);
    }

}

