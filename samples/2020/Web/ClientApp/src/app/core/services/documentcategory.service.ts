import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DocumentCategoryDto } from '../models/documentcategory/documentcategory-dto';
import { DocumentCategoryStatusDto } from '../models/documentcategory/documentcategory-status-dto';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class DocumentCategoryService {

  constructor(private apiService: ApiService) { }

  getDocumentCategories(): Observable<DocumentCategoryDto[]> {
    return this.apiService.get<DocumentCategoryDto[]>('DocumentCategory');
  }

  getDocumentCategoriesByStatusAndGroupId(status: string, groupId:number): Observable<DocumentCategoryDto[]> {
    return this.apiService.get<DocumentCategoryDto[]>(`DocumentCategory/Filtered/?statusFilter=${status}&groupId=${groupId}`);
  }

  getPublishedDocumentCategories(groupId:number): Observable<DocumentCategoryDto[]> {
    return this.apiService.get<DocumentCategoryDto[]>(`DocumentCategory/getpublisheddocument/${groupId}`);
  }

  getDocumentCategory(id: number): Observable<DocumentCategoryDto> {
    return this.apiService.get<DocumentCategoryDto>(`DocumentCategory/${id}`);
  }

  createDocumentCategory(dto: DocumentCategoryDto) {
    return this.apiService.post('DocumentCategory', dto);
  }

  updateDocumentCategory(dto: DocumentCategoryDto) {
    return this.apiService.put(`DocumentCategory/${dto.id}`, dto);
  }

  deleteDocumentCategory(id: number) {
    return this.apiService.delete(`DocumentCategory/${id}`);
  }

  getDocumentCategoryStatuses(): Observable<DocumentCategoryStatusDto[]> {
    return this.apiService.get<DocumentCategoryStatusDto[]>(`DocumentCategory/Statuses`);
  }
}

