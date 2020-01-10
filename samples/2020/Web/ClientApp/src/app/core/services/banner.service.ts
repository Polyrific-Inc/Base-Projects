///OpenCatapultModelId:924
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BannerDto } from '../models/banner/banner-dto';
import { ApiService } from './api.service';
import { BannerStatusDto } from '../models/banner/banner-status-dto';
import { BannerFilterDto } from '../models/banner/banner-filter-dto';
import { BannerFilterResultDto } from '../models/banner/banner-filter-result-dto';

@Injectable({
  providedIn: 'root'
})
export class BannerService {

  constructor(private apiService: ApiService) { }

  getBanners(): Observable<BannerDto[]> {
    return this.apiService.get<BannerDto[]>('Banner');
  }

  getBanner(id: number): Observable<BannerDto> {
    return this.apiService.get<BannerDto>(`Banner/${id}`);
  }

  createBanner(dto: BannerDto) {
    return this.apiService.post('Banner', dto);
  }

  updateBanner(dto: BannerDto) {
    return this.apiService.put(`Banner/${dto.id}`, dto);
  }

  deleteBanner(id: number) {
    return this.apiService.delete(`Banner/${id}`);
  }

  archiveBanner(id: number) {
    return this.apiService.post(`Banner/archive/${id}`);
  }

  getBannerStatus():Observable<BannerStatusDto[]> {
    return this.apiService.get<BannerStatusDto[]>(`Banner/getstatus`);
  }

  getActiveBanners():Observable<BannerDto[]>{
    return this.apiService.get<BannerDto[]>(`Banner/GetActiveBanners`);
  }

  cleanupBannerImages() {
    return this.apiService.post(`Banner/CleanupBannerImages`);
  }

  getFilteredBanners(filterParameter: BannerFilterDto): Observable<BannerFilterResultDto> {
    return this.apiService.post<BannerFilterResultDto>('Banner/FilterBanners', filterParameter);
  }
}

