import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserComponent } from './user/user.component';
import { UserNewComponent } from './user-new/user-new.component';
import { UserEditComponent } from './user-edit/user-edit.component';
import { UserErrorComponent } from './user-error/user-error.component';
import { UserResolverService } from './services/user-resolver.service';

const routes: Routes = [
  {
    path: '',
    component: UserComponent
  },
  {
    path: 'new',
    component: UserNewComponent
  },
  {
    path: ':id',
    component: UserEditComponent,
    resolve: {
      user: UserResolverService
    }
  },
  {
    path: ':id/error', component: UserErrorComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
