import { Component, OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Validators, FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../core/auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loading = false;
  returnUrl: string;
  error = '';
  loginForm = this.fb.group({
    userName: [null, Validators.required],
    password: [null, Validators.required]
  });
  submitted = false;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService) {
    if (this.authService.currentUserValue) {
      this.router.navigate(['/']);
    }
  }

  ngOnInit() {
    // get return url from route parameters or default to '/'
    this.returnUrl = '/';
    this.route.queryParams.subscribe(data => {
      if (data.returnUrl) {
        this.returnUrl = data.returnUrl;
      }
    });
  }

  onSubmit() {
    this.submitted = true;
    if (this.loginForm.valid) {
      this.loading = true;
      this.authService.login(this.loginForm.value)
        .subscribe(
          () => {
            this.router.navigate([this.returnUrl]);
          },
          (err: HttpErrorResponse) => {
            if (err.error && typeof err.error === 'string') {
              this.error = err.error;
            } else {
              this.error = err.message;
            }
            this.loading = false;
          });
    }
  }

  // convenience getter for easy access to form fields
  get f() { return this.loginForm.controls; }
}
