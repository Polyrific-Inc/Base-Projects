import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { UserService } from '@app/core/services/user.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  loading = false;
  resetPasswordDone: boolean;
  username: string;
  token: string;

  resetPasswordForm: FormGroup;
  submitted = false;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private route: ActivatedRoute) {
    }

  ngOnInit() {
    this.username = this.route.snapshot.queryParams['username'];
    this.token = this.route.snapshot.queryParams['token'];

    this.resetPasswordForm = this.fb.group({
      token: [this.token, Validators.required],
      newPassword: [null, Validators.compose([Validators.required, Validators.minLength(6)])],
      confirmNewPassword: null
    }, {validators: this.checkPasswords});
  }

  onSubmit() {
    this.submitted = true;
    if (this.resetPasswordForm.valid) {
      this.loading = true;
      this.userService.resetPassword(this.username, this.resetPasswordForm.value)
        .subscribe(
            data => {
              this.resetPasswordDone = true;
              this.loading = false;
            });
    }
  }

  checkPasswords(group: FormGroup) { // here we have the 'passwords' group
    const pass = group.controls.newPassword.value;
    const confirmPass = group.controls.confirmNewPassword.value;

    return pass === confirmPass ? null : { notSame: true };
  }

  isPasswordMatchInvalid() {
    return this.resetPasswordForm.hasError('notSame');
  }

  // convenience getter for easy access to form fields
  get f() { return this.resetPasswordForm.controls; }

}
