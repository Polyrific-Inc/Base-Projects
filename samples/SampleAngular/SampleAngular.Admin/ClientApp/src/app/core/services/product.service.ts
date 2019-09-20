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

  getProduct(productId: number): Observable<ProductDto> {
    return this.apiService.get<ProductDto>(`product/${productId}`);
  }

  createProduct(product: ProductDto) {
    return this.apiService.post('product', product);
  }

  updateProduct(product: ProductDto) {
    return this.apiService.put(`product/${product.id}`, product);
  }

  deleteProduct(productId: number) {
    return this.apiService.delete(`product/${productId}`);
  }
}
