import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-product-error',
  templateUrl: './product-error.component.html',
  styleUrls: ['./product-error.component.css']
})
export class ProductErrorComponent implements OnInit {
  productId: number;

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.productId = this.route.snapshot.params.id;
  }

}
