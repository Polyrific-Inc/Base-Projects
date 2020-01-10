import { Injectable } from '@angular/core';
import { ApiService } from './api.service';

@Injectable()
export class FileService {

    constructor(private apiService: ApiService) { }


    upload(file: any) {
        let input = new FormData();
        input.append("filesData", file);
        return this.apiService.post('Document/UploadFiles', input);
    }
}