import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { CorporateDirectoryDto } from '../models/user/corporate-directory-dto';
import { CorporateDirectoryFilterDto } from '../models/user/corporate-directory-filter-dto';
import { CorporateDirectoryFilterResultDto } from '../models/user/corporate-directory-filter-result-dto';

@Injectable({
    providedIn: 'root'
})

export class UserProfileService {

    constructor(private apiService: ApiService) { }

    getUserProfiles(): Observable<CorporateDirectoryDto> {
        return this.apiService.get<CorporateDirectoryDto>('UserProfile');
    }

    getFilteredUserProfiles(filter: CorporateDirectoryFilterDto): Observable<CorporateDirectoryFilterResultDto> {
        return this.apiService.get<CorporateDirectoryFilterResultDto>(`UserProfile/Filtered/?skip=${filter.skip}&take=${filter.take}&firstNameFirstLetter=${filter.firstNameFirstLetter}`);
    }
}