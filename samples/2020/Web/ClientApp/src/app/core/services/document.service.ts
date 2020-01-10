import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DocumentDto } from '../models/document/document-dto';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class DocumentService {

  constructor(private apiService: ApiService) { }

  getDocuments(): Observable<DocumentDto[]> {
    return this.apiService.get<DocumentDto[]>('Document');
  }

  getDocument(id: number): Observable<DocumentDto> {
    return this.apiService.get<DocumentDto>(`Document/${id}`);
  }

  createDocument(dto: DocumentDto) {
    return this.apiService.post('Document', dto);
  }

  updateDocument(dto: DocumentDto) {
    return this.apiService.put(`Document/${dto.id}`, dto);
  }

  deleteDocument(id: number) {
    return this.apiService.delete(`Document/${id}`);
  }

  getDocumentsByDocumentCategoryId(documentCategoryId: number): Observable<DocumentDto[]> {
    return this.apiService.get<DocumentDto[]>(`documentcategory/${documentCategoryId}/documents`);
  }
}

