import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EventReservationDto } from '../models/eventreservation/eventreservation-dto';
import { ApiService } from './api.service';
import { EventReservationCustomDto } from '../models/eventreservation/eventreservation-custom-dto';
import { ExportDto } from '../models/eventreservation/export-dto';

@Injectable({
  providedIn: 'root'
})
export class EventReservationService {

  constructor(private apiService: ApiService) { }

  getEventReservations(): Observable<EventReservationCustomDto[]> {
    return this.apiService.get<EventReservationCustomDto[]>('EventReservation');
  }

  getEventReservation(id: number): Observable<EventReservationDto> {
    return this.apiService.get<EventReservationDto>(`EventReservation/${id}`);
  }

  createEventReservation(dto: EventReservationDto) {
    return this.apiService.post('EventReservation', dto);
  }

  updateEventReservation(dto: EventReservationDto) {
    return this.apiService.put(`EventReservation/${dto.id}`, dto);
  }

  deleteEventReservation(id: number) {
    return this.apiService.delete(`EventReservation/${id}`);
  }

  registerEvent(eventId: number) {
    return this.apiService.post(`registerevent/${eventId}`);
  }

  getEventReservationsByReserverIdAndEventId(eventId:number): Observable<EventReservationDto[]> {
      return this.apiService.get<EventReservationDto[]>(`getbyreserveridandeventid/${eventId}`);
  }

  getEventReservationsByEventId(eventId:number): Observable<EventReservationCustomDto[]> {
    return this.apiService.get<EventReservationCustomDto[]>(`getbyeventid/${eventId}`);
  }

  exportReservationByEventId(eventId:number):Observable<ExportDto> {
    return this.apiService.get<ExportDto>(`exporteventreservation/${eventId}`);
  }

  registerEventByAdmin(eventId:number, reserverName:string, reserverEmail:string) {
    var dto = {
      EventId: eventId,
      ReserverName: reserverName,
      reserverEmail: reserverEmail,
      ReserverId: reserverEmail.split('@')[0]
    }
    return this.apiService.post('EventReservation/registereventbyadmin', dto);
  }

  deleteMultipleEventReservation(eventIds: number[]) {
    return this.apiService.post(`EventReservation/unregisteredevents`,eventIds);
  }
}

