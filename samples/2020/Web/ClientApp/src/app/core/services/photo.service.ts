import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PhotoDto } from '../models/photo/photo-dto';
import { ApiService } from './api.service';
import { Guid } from 'guid-typescript';

@Injectable({
  providedIn: 'root'
})
export class PhotoService {

  constructor(private apiService: ApiService) { }

  getPhotos(): Observable<PhotoDto[]> {
    return this.apiService.get<PhotoDto[]>('Photo');
  }

  getPhotosByGalleryId(galleryId: number): Observable<PhotoDto[]> {
    return this.apiService.get<PhotoDto[]>(`Gallery/${galleryId}/Photo`);
  }

  getPhoto(id: number): Observable<PhotoDto> {
    return this.apiService.get<PhotoDto>(`Photo/${id}`);
  }

  createPhoto(dto: PhotoDto) {
    return this.apiService.post('Photo', dto);
  }

  updatePhoto(dto: PhotoDto) {
    return this.apiService.put(`Photo/${dto.id}`, dto);
  }

  deletePhoto(id: number) {
    return this.apiService.delete(`Photo/${id}`);
  }

  updatePhotoOrder(photoOrder: {id: number, indexOrder: number}[]){
    return this.apiService.post('Photo/updatephotoorder', photoOrder);
  }

  cleanupGalleryPhotos(galleryGuid: string){
    return this.apiService.post(`Photo/CleanupGalleryPhotos/${galleryGuid}`);
  }


}

