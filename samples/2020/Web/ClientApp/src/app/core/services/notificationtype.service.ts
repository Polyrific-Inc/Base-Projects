///OpenCatapultModelId:915
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { NotificationTypeDto } from '../models/notificationtype/notificationtype-dto';
import { ApiService } from './api.service';
import { NotificationTypeFilterResultDto } from '../models/notificationtype/notificationtype-filter-result-dto';

@Injectable({
  providedIn: 'root'
})
export class NotificationTypeService {

  constructor(private apiService: ApiService) { }

  getNotificationTypes(): Observable<NotificationTypeDto[]> {
    return this.apiService.get<NotificationTypeDto[]>('NotificationType');
  }

  getNotificationType(id: number): Observable<NotificationTypeDto> {
    return this.apiService.get<NotificationTypeDto>(`NotificationType/${id}`);
  }

  createNotificationType(dto: NotificationTypeDto) {
    return this.apiService.post('NotificationType', dto);
  }

  updateNotificationType(dto: NotificationTypeDto) {
    return this.apiService.put(`NotificationType/${dto.id}`, dto);
  }

  deleteNotificationType(id: number) {
    return this.apiService.delete(`NotificationType/${id}`);
  }

  getFilteredNotificationTypes(filter: {skip: number, take:number}): Observable<NotificationTypeFilterResultDto> {
    return this.apiService.get<NotificationTypeFilterResultDto>(`NotificationType/Filtered/?skip=${filter.skip}&take=${filter.take}`);
  }

}

