import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { UserService } from '@app/core/services/user.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {
  loading = false;
  resetPasswordRequested: boolean;

  forgotPasswordForm = this.fb.group({
    email: [null, Validators.compose([
      Validators.required, Validators.email])]
  });

  submitted: boolean;

  constructor(
    private fb: FormBuilder,
    private userService: UserService) {
    }

  ngOnInit() {
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.forgotPasswordForm.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }

  onSubmit() {
    this.submitted = true;
    if (this.forgotPasswordForm.valid) {
        this.loading = true;
      this.userService.requestResetPassword(this.forgotPasswordForm.value.email)
        .subscribe(
            data => {
              this.resetPasswordRequested = true;
              this.loading = false;
            });
    }
  }

  // convenience getter for easy access to form fields
  get f() { return this.forgotPasswordForm.controls; }
}
