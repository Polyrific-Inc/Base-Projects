import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProductRoutingModule } from './product-routing.module';
import { ProductComponent } from './product/product.component';
import { ProductCreateComponent } from './product-create/product-create.component';
import { ProductEditComponent } from './product-edit/product-edit.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ProductErrorComponent } from './product-error/product-error.component';
import { ProductResolverService } from './services/product-resolver.service';


@NgModule({
  declarations: [ProductComponent, ProductCreateComponent, ProductEditComponent, ProductErrorComponent],
  imports: [
    CommonModule,
    ProductRoutingModule,
    ReactiveFormsModule
  ],
  providers: [
    ProductResolverService
  ]
})
export class ProductModule { }
