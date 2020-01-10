///OpenCatapultModelId:948
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DocumentGroupDto } from '../models/documentgroup/documentgroup-dto';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class DocumentGroupService {

  constructor(private apiService: ApiService) { }

  getDocumentGroups(): Observable<DocumentGroupDto[]> {
    return this.apiService.get<DocumentGroupDto[]>('DocumentGroup');
  }

  getDocumentGroup(id: number): Observable<DocumentGroupDto> {
    return this.apiService.get<DocumentGroupDto>(`DocumentGroup/${id}`);
  }

  createDocumentGroup(dto: DocumentGroupDto) {
    return this.apiService.post('DocumentGroup', dto);
  }

  updateDocumentGroup(dto: DocumentGroupDto) {
    return this.apiService.put(`DocumentGroup/${dto.id}`, dto);
  }

  deleteDocumentGroup(id: number) {
    return this.apiService.delete(`DocumentGroup/${id}`);
  }
}

