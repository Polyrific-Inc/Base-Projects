import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserRoutingModule } from './user-routing.module';
import { UserComponent } from './user/user.component';
import { UserFormComponent } from './user-form/user-form.component';
import { UserNewComponent } from './user-new/user-new.component';
import { UserEditComponent } from './user-edit/user-edit.component';
import { UserErrorComponent } from './user-error/user-error.component';
import { UserResolverService } from './services/user-resolver.service';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [UserComponent, UserFormComponent, UserNewComponent, UserEditComponent, UserErrorComponent],
  providers: [
    UserResolverService
  ],
  imports: [
    CommonModule,
    UserRoutingModule,
    ReactiveFormsModule
  ]
})
export class UserModule { }
