///OpenCatapultModelId:925
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { QuickLinkDto } from '../models/quicklink/quicklink-dto';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class QuickLinkService {

  constructor(private apiService: ApiService) { }

  getQuickLinks(): Observable<QuickLinkDto[]> {
    return this.apiService.get<QuickLinkDto[]>('QuickLink');
  }

  getQuickLink(id: number): Observable<QuickLinkDto> {
    return this.apiService.get<QuickLinkDto>(`QuickLink/${id}`);
  }

  createQuickLink(dto: QuickLinkDto) {
    return this.apiService.post('QuickLink', dto);
  }

  updateQuickLink(dto: QuickLinkDto) {
    return this.apiService.put(`QuickLink/${dto.id}`, dto);
  }

  deleteQuickLink(id: number) {
    return this.apiService.delete(`QuickLink/${id}`);
  }
}

