import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProductRoutingModule } from './product-routing.module';
import { ProductComponent } from './product/product.component';
import { ProductFormComponent } from './product-form/product-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ProductNewComponent } from './product-new/product-new.component';
import { ProductEditComponent } from './product-edit/product-edit.component';
import { ProductErrorComponent } from './product-error/product-error.component';
import { ProductResolverService } from './services/product-resolver.service';

@NgModule({
  declarations: [
    ProductComponent,
    ProductFormComponent,
    ProductNewComponent,
    ProductEditComponent,
    ProductErrorComponent
  ],
  providers: [
    ProductResolverService
  ],
  imports: [
    CommonModule,
    ProductRoutingModule,
    ReactiveFormsModule
  ]
})
export class ProductModule { }
