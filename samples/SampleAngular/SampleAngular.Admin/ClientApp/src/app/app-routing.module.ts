import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { HomeLayoutComponent } from './home-layout/home-layout.component';
import { AuthGuard } from '@app/core/auth/auth.guard';
import { AuthorizePolicy } from '@app/core/auth/authorize-policy';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';
import { LoginComponent } from './login/login.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';

const routes: Routes = [
  {
    path: '',
    //component: HomeLayoutComponent,
    canActivateChild: [AuthGuard],
    children: [
      { path: '', component: HomeComponent, pathMatch: 'full' },
      {
        path: 'product', loadChildren: './product/product.module#ProductModule',
        data: { authPolicy: AuthorizePolicy.UserRoleAdminAccess }
      },
      {
        path: 'unauthorized',
        component: UnauthorizedComponent
      },
    ]
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'forgot-password',
    component: ForgotPasswordComponent
  },
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
