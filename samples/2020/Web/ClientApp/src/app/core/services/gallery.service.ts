import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GalleryDto } from '../models/gallery/gallery-dto';
import { ApiService } from './api.service';
import { GalleryCustomDto } from '@app/core/models/gallery/gallery-custom-dto';
import { GalleryFilterDto } from '../models/gallery/gallery-filter-dto';
import { GalleryFilterResultDto } from '../models/gallery/gallery-filter-result-dto';
import { GalleryStatusDto } from '../models/gallery/gallery-status-dto';

@Injectable({
  providedIn: 'root'
})
export class GalleryService {

  constructor(private apiService: ApiService) { }

  getGalleries(): Observable<GalleryCustomDto[]> {
    return this.apiService.get<GalleryCustomDto[]>(`Gallery`);
  }

  getFeaturedAlbums(): Observable<GalleryCustomDto[]> {
    return this.apiService.get<GalleryCustomDto[]>(`Gallery/GetFeaturedAlbums`);
  }

  getGallery(id: number): Observable<GalleryDto> {
    return this.apiService.get<GalleryDto>(`Gallery/${id}`);
  }

  getGalleryByUrl(url: string): Observable<GalleryDto> {
    return this.apiService.get<GalleryDto>(`Gallery/view/${url}`);
  }

  createGallery(dto: GalleryCustomDto) {
    return this.apiService.post('Gallery', dto);
  }

  updateGallery(dto: GalleryCustomDto) {
    return this.apiService.put(`Gallery/${dto.id}`, dto);
  }

  deleteGallery(id: number) {
    return this.apiService.delete(`Gallery/${id}`);
  }

  setCoverImage(galleryId: number, photoId: number) {
    var dto = {
      GalleryId:galleryId,
      PhotoId:photoId
    }
    return this.apiService.post('Gallery/SetCoverImage', dto);
  }

  getFilteredAndSortedGalleries(filterParameter:GalleryFilterDto): Observable<GalleryFilterResultDto> {
    return this.apiService.post<GalleryFilterResultDto>(`Gallery/FilterAndSortGalleries`, filterParameter);
  }

  getFilteredAndSortedPublishedGalleries(filterParameter: GalleryFilterDto): Observable<GalleryFilterResultDto> {
      return this.apiService.post<GalleryFilterResultDto>(`Gallery/FilterAndSortPublishedGalleries`, filterParameter);
  }

  getPublishedGallery(skip: number, take: number): Observable<GalleryCustomDto[]> {
    return this.apiService.get<GalleryCustomDto[]>(`Gallery/getpublishedgallery?skip=${skip}&take=${take}`);
  }

  getGalleryStatuses(): Observable<GalleryStatusDto[]> {
    return this.apiService.get<GalleryStatusDto[]>(`Gallery/Statuses`);
  }
}

