///OpenCatapultModelId:914
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { NotificationDto } from '../models/notification/notification-dto';
import { NotificationCustomDto } from '../models/notification/notification-custom-dto';
import { ApiService } from './api.service';
import { NotificationFilterDto } from "../models/notification/notification-filter-dto";
import { NotificationFilterResultDto } from "../models/notification/notification-filter-result-dto";

@Injectable({
    providedIn: 'root'
})
export class NotificationService {

    constructor(private apiService: ApiService) { }

    getNotifications(): Observable<NotificationCustomDto[]> {
        return this.apiService.get<NotificationCustomDto[]>('Notification');
    }

    getNotification(id: number): Observable<NotificationDto> {
        return this.apiService.get<NotificationDto>(`Notification/${id}`);
    }

    createNotification(dto: NotificationDto) {
        return this.apiService.post('Notification', dto);
    }

    updateNotification(dto: NotificationDto) {
        return this.apiService.put(`Notification/${dto.id}`, dto);
    }

    deleteNotification(id: number) {
        return this.apiService.delete(`Notification/${id}`);
    }

    getFilteredNotifications(filter: NotificationFilterDto): Observable<NotificationFilterResultDto> {
        return this.apiService.get<NotificationFilterResultDto>(`Notification/Filtered/?skip=${filter.skip}&take=${filter.take}&includeAdmin=${filter.includeAdmin}`);
    }

    getFilteredNotificationsbyUserId(): Observable<NotificationFilterResultDto> {
        return this.apiService.get<NotificationFilterResultDto>(`Notification/FilteredByUser`);
    }

    getNotificationsByNotificationTypeId(notificationTypeId: number): Observable<NotificationFilterResultDto> {
        return this.apiService.get<NotificationFilterResultDto>(`Notification/FilteredByNotificationType/${notificationTypeId}`);
    }
}

