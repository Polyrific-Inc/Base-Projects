import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ProductDto } from '../models/product/product-dto';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private apiService: ApiService) { }

  getProducts(): Observable<ProductDto[]> {
    return this.apiService.get<ProductDto[]>('product');
  }
}
