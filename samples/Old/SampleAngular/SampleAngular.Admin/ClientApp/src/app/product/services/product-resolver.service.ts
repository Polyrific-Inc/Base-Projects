import { Injectable } from '@angular/core';
import { ProductDto } from '@app/core/models/product/product-dto';
import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { mergeMap } from 'rxjs/operators';
import { of, EMPTY, Observable } from 'rxjs';
import { ProductService } from '@app/core/services/product.service';

@Injectable()
export class ProductResolverService implements Resolve<ProductDto> {

  constructor(
    private productService: ProductService,
    private router: Router
  ) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): ProductDto | Observable<ProductDto> | Promise<ProductDto> {
    const productId = +route.params.id;

    return this.productService.getProduct(productId)
      .pipe(mergeMap(job => {
        if (job) {
          return of(job);
        } else {
          this.router.navigateByUrl(`/product/${productId}/error`);
          return EMPTY;
        }
      }));
  }
}
