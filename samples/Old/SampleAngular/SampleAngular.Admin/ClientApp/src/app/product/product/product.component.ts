import { Component, OnInit } from '@angular/core';
import { ProductDto } from '@app/core/models/product/product-dto';
import { ProductService } from '@app/core/services/product.service';
import { ConfirmationDialogService } from '@app/shared/services/confirmation-dialog.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {
  products: ProductDto[];

  constructor(private productService: ProductService, private dialog: ConfirmationDialogService) { }

  ngOnInit() {
    this.getProducts();
  }

  onDeleteClicked(productId: number) {
    this.dialog.open('Confirm Delete Product', `Are you sure you want to delete product ${productId}?`).then(result => {
      if (result) {
        this.productService.deleteProduct(productId).subscribe(() => {
          this.getProducts();
        });
      }
    }).catch(() => console.log('dismissed'));
  }

  getProducts() {
    this.productService.getProducts().subscribe(data => this.products = data);
  }

}
