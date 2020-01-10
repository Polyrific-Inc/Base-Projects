import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { NewsArticleCustomDto } from '../models/newsarticle/newsarticlecustom-dto';
import { NewsArticleDto } from '../models/newsarticle/newsarticle-dto';
import { NewsArticleStatusDto } from '../models/newsarticle/newsarticle-status-dto';
import { NewsArticleFilterDto } from '../models/newsarticle/newsarticle-filter-dto';
import { NewsArticleFilterResultDto } from '../models/newsarticle/newsarticle-filter-result-dto';

@Injectable({
  providedIn: 'root'
})
export class NewsArticleService {

  constructor(private apiService: ApiService) { }

  getNewsArticles(): Observable<NewsArticleDto[]> {
    return this.apiService.get<NewsArticleDto[]>('NewsArticle');
  }

  getNewsArticle(id: number): Observable<NewsArticleDto> {
    return this.apiService.get<NewsArticleDto>(`NewsArticle/${id}`);
  }

  getNewsArticleByUrl(url: string): Observable<NewsArticleDto> {
    return this.apiService.get<NewsArticleDto>(`NewsArticle/view/${url}`);
  }

  createNewsArticle(dto: NewsArticleDto) {
    return this.apiService.post('NewsArticle', dto);
  }

  updateNewsArticle(dto: NewsArticleDto) {
    return this.apiService.put(`NewsArticle/${dto.id}`, dto);
  }

  softDeleteNewsArticle(id: number) {
    return this.apiService.post(`NewsArticle/softdelete/${id}`);
  }

  getPublishedArticle(): Observable<NewsArticleCustomDto[]> {
    return this.apiService.get<NewsArticleCustomDto[]>(`NewsArticle/publishedarticle`);
  }

  createNewsArticleCustom(dto: NewsArticleCustomDto) {
    return this.apiService.post('NewsArticle', dto);
  }

  updateNewsArticleCustom(dto: NewsArticleCustomDto) {
    return this.apiService.put(`NewsArticle/${dto.id}`, dto);
  }

  getFilteredNewsArticles(filter: NewsArticleFilterDto): Observable<NewsArticleFilterResultDto> {
    return this.apiService.get<NewsArticleFilterResultDto>(`NewsArticle/Filtered/?statusFilter=${filter.status}&skip=${filter.skip}&take=${filter.take}&dateNow=${filter.dateNow}`);
  }

  getNewsArticleStatuses(): Observable<NewsArticleStatusDto[]> {
    return this.apiService.get<NewsArticleStatusDto[]>(`NewsArticle/Statuses`);
  }

  cleanupNewsArticleImages() {
      return this.apiService.post(`NewsArticle/CleanupNewsArticleImages`);
  }
}

