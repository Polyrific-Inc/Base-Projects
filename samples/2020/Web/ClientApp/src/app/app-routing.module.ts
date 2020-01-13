import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from '@app/core/auth/auth.guard';
import { AuthorizePolicy } from '@app/core/auth/authorize-policy';
import { ContentLayoutComponent } from './shared/components/layout/content-layout/content-layout.component';
import { LoginComponent } from './login/login.component';

const routes: Routes = [
  {
    path: '',
    canActivateChild: [AuthGuard],
    component: ContentLayoutComponent,
    children: [
      {
        path: '', component: HomeComponent, pathMatch: 'full',
        data: {
          title: 'Dashboard',
          breadcrumb: 'Dashboard'
        }
      },
      {
        path: 'product', loadChildren: './product/product.module#ProductModule'
      }
    ]
  },
  {
      path: 'login',
      component: LoginComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
