///OpenCatapultModelId:925
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { SearchDto } from '@app/core/models/search/search-dto'

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  constructor(private apiService: ApiService) { }

  search(searchString: string): Observable<SearchDto> {
    return this.apiService.get<SearchDto>(`Search/${searchString}`);
  }

  searchByEntity(keyword:string, entity:string): Observable<SearchDto>{
    return this.apiService.get<SearchDto>(`Search/${keyword}?searchEntity=${entity}`);
  }
}
