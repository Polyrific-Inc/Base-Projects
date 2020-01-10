///OpenCatapultModelId:945
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserPhotoDto } from '../models/userphoto/userphoto-dto';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class UserPhotoService {

  constructor(private apiService: ApiService) { }

  getUserPhotos(): Observable<UserPhotoDto[]> {
    return this.apiService.get<UserPhotoDto[]>('UserPhoto');
  }

  getUserPhoto(id: number): Observable<UserPhotoDto> {
    return this.apiService.get<UserPhotoDto>(`UserPhoto/${id}`);
  }

  createUserPhoto(dto: UserPhotoDto) {
    return this.apiService.post('UserPhoto', dto);
  }

  updateUserPhoto(dto: UserPhotoDto) {
    return this.apiService.put(`UserPhoto/${dto.id}`, dto);
  }

  deleteUserPhoto(id: number) {
    return this.apiService.delete(`UserPhoto/${id}`);
  }
}

