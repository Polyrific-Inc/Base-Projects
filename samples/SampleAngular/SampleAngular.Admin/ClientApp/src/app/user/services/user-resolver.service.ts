import { Injectable } from '@angular/core';
import { UserDto } from '@app/core/models/user/user-dto';
import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { mergeMap } from 'rxjs/operators';
import { of, EMPTY, Observable } from 'rxjs';
import { UserService } from '@app/core/services/user.service';

@Injectable()
export class UserResolverService implements Resolve<UserDto> {

  constructor(
    private userService: UserService,
    private router: Router
  ) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): UserDto | Observable<UserDto> | Promise<UserDto> {
    const userId = +route.params.id;

    return this.userService.getUser(userId)
      .pipe(mergeMap(job => {
        if (job) {
          return of(job);
        } else {
          this.router.navigateByUrl(`/user/${userId}/error`);
          return EMPTY;
        }
      }));
  }
}
