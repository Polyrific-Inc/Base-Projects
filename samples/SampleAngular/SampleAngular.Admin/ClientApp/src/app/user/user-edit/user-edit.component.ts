import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { UserService } from '@app/core/services/user.service';
import { Router, ActivatedRoute } from '@angular/router';
import { UserDto } from '@app/core/models/user/user-dto';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {
  userForm: FormGroup;
  submitted = false;
  user: UserDto;

  constructor(
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private router: Router,
    private userService: UserService) { }

  ngOnInit() {
    this.userForm = this.formBuilder.group({
      id: [{ value: '', disabled: true }],
      userName: ['', Validators.compose([Validators.required, Validators.email])],
      email: [{value: '', disabled: true}, Validators.compose([Validators.required, Validators.email])],
      firstName: '',
      lastName: ['', Validators.required],
      role: ['', Validators.required]
    });
    this.route.data.subscribe((data: { user: UserDto }) => {
      this.user = data.user;
      this.userForm.patchValue(this.user);
    });
  }

  onSubmit() {
    this.submitted = true;
    if (this.userForm.invalid) {
      return;
    }

    this.userService.updateUser({
      id: this.user.id,
      ...this.userForm.value
    }).subscribe(() => {
      this.router.navigateByUrl('/user');
    });
  }
}
