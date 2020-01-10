///OpenCatapultModelId:943
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EmployeeHighlightDto } from '../models/employeehighlight/employeehighlight-dto';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class EmployeeHighlightService {

  constructor(private apiService: ApiService) { }

  getEmployeeHighlights(): Observable<EmployeeHighlightDto[]> {
    return this.apiService.get<EmployeeHighlightDto[]>('EmployeeHighlight');
  }

  getEmployeeHighlight(id: number): Observable<EmployeeHighlightDto> {
    return this.apiService.get<EmployeeHighlightDto>(`EmployeeHighlight/${id}`);
  }

  createEmployeeHighlight(dto: EmployeeHighlightDto) {
    return this.apiService.post('EmployeeHighlight', dto);
  }

  updateEmployeeHighlight(dto: EmployeeHighlightDto) {
    return this.apiService.put(`EmployeeHighlight/${dto.id}`, dto);
  }

  deleteEmployeeHighlight(id: number) {
    return this.apiService.delete(`EmployeeHighlight/${id}`);
  }
}

