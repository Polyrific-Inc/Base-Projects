import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ProductService } from '../../core/services/product.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-product-create',
  templateUrl: './product-create.component.html',
  styleUrls: ['./product-create.component.scss']
})
export class ProductCreateComponent implements OnInit {
  productForm: FormGroup;
  submitted = false;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private productService: ProductService) { }

  ngOnInit() {
    this.productForm = this.formBuilder.group({
      name: ['', Validators.required]
    });
  }

  onSubmit() {
    this.submitted = true;
    if (this.productForm.invalid) {
      return;
    }

    this.productService.createProduct(this.productForm.value).subscribe(() => {
      this.router.navigateByUrl('/product');
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.productForm.controls; }
}
