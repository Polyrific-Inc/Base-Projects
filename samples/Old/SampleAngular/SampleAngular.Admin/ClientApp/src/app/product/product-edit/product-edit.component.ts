import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ProductService } from '@app/core/services/product.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ProductDto } from '@app/core/models/product/product-dto';

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.css']
})
export class ProductEditComponent implements OnInit {
  productForm: FormGroup;
  submitted = false;
  product: ProductDto;

  constructor(
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private router: Router,
    private productService: ProductService) { }

  ngOnInit() {
    this.productForm = this.formBuilder.group({
      id: [{ value: '', disabled: true }],
      name: ['', Validators.required]
    });
    this.route.data.subscribe((data: { product: ProductDto }) => {
      this.product = data.product;
      this.productForm.patchValue(this.product);
    });
  }

  onSubmit() {
    this.submitted = true;
    if (this.productForm.invalid) {
      return;
    }

    this.productService.updateProduct({
      id: this.product.id,
      ...this.productForm.value
    }).subscribe(() => {
      this.router.navigateByUrl('/product');
    });
  }
}
