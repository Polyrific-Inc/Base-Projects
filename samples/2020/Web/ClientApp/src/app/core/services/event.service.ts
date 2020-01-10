import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EventDto } from '../models/event/event-dto';
import { ApiService } from './api.service';
import { EventFilterResultDto } from "../models/event/event-filter-result-dto";
import { EventFilterDto } from "../models/event/event-filter-dto";
import { EventStatusDto } from "../models/event/event-status-dto";

@Injectable({
  providedIn: 'root'
})
export class EventService {

  constructor(private apiService: ApiService) { }

  getEvents(): Observable<EventDto[]> {
    return this.apiService.get<EventDto[]>(`Event`);
  }

  getEvent(id: number): Observable<EventDto> {
    return this.apiService.get<EventDto>(`Event/${id}`);
  }

  getEventByUrl(url: string): Observable<EventDto> {
    return this.apiService.get<EventDto>(`Event/view/${url}`);
  }

  createEvent(dto: EventDto) {
    return this.apiService.post('Event', dto);
  }

  updateEvent(dto: EventDto) {
    return this.apiService.put(`Event/${dto.id}`, dto);
  }

  deleteEvent(id: number) {
    return this.apiService.delete(`Event/${id}`);
  }

  getActiveEvents(skip: number, take: number): Observable<EventFilterResultDto> {
    return this.apiService.get<EventFilterResultDto>(`Event/activeevents?skip=${skip}&take=${take}`);
  }

  getActiveEventsForUser(filter: EventFilterDto): Observable<EventFilterResultDto> {
    return this.apiService.get<EventFilterResultDto>(`Event/activeevents?skip=${filter.skip}&take=${filter.take}`);
  }

  getEventsByReserverId(): Observable<EventDto[]> {
    return this.apiService.get<EventDto[]>(`Event/geteventsbyreserverid`);
  }

  archiveEvent(id: number) {
    return this.apiService.post(`Event/Archive/${id}`);
  }

  getFilteredEvents(filter: EventFilterDto): Observable<EventFilterResultDto> {
    return this.apiService.get<EventFilterResultDto>(`Event/Filtered/?statusFilter=${filter.status}&skip=${filter.skip}&take=${filter.take}&dateNow=${filter.dateNow}`);
  }

  getEventStatuses(): Observable<EventStatusDto[]> {
    return this.apiService.get<EventStatusDto[]>(`Event/Statuses`);
  }

  cleanupEventImages() {
      return this.apiService.post(`Event/CleanupEventImages`);
  }
}

