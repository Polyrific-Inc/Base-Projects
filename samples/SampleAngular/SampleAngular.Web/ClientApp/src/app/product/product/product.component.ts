import { Component, OnInit } from '@angular/core';
import { ProductDto } from '@app/core/models/product/product-dto';
import { ProductService } from '@app/core/services/product.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {
  products: ProductDto[];

  constructor(private productService: ProductService) { }

  ngOnInit() {
    this.productService.getProducts().subscribe(data => this.products = data);
  }

}
