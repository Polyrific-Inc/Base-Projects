import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductComponent } from './product/product.component';
import { ProductNewComponent } from './product-new/product-new.component';
import { ProductEditComponent } from './product-edit/product-edit.component';
import { ProductErrorComponent } from './product-error/product-error.component';
import { ProductResolverService } from './services/product-resolver.service';

const routes: Routes = [
  {
    path: '',
    component: ProductComponent
  },
  {
    path: 'new',
    component: ProductNewComponent
  },
  {
    path: ':id',
    component: ProductEditComponent,
    resolve: {
      product: ProductResolverService
    }
  },
  {
    path: ':id/error', component: ProductErrorComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProductRoutingModule { }
