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
    const id = +route.params.id;

    return this.productService.getProduct(id)
      .pipe(mergeMap(item => {
        if (item) {
          return of(item);
        } else {
          this.router.navigateByUrl(`/product/${id}/error`);
          return EMPTY;
        }
      }));
  }
}

