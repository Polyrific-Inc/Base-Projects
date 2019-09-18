import { Component, OnInit, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-product-form',
  templateUrl: './product-form.component.html',
  styleUrls: ['./product-form.component.css']
})
export class ProductFormComponent implements OnInit {
  @Input() productForm: FormGroup;
  @Input() submitted: boolean;

  constructor(
  ) { }

  ngOnInit() {
  }

  // convenience getter for easy access to form fields
  get f() { return this.productForm.controls; }
}
