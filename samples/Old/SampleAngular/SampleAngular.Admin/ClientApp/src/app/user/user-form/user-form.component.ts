import { Component, OnInit, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { userRoles } from '@app/core/constants/user-roles';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.css']
})
export class UserFormComponent implements OnInit {
  @Input() userForm: FormGroup;
  @Input() submitted: boolean;
  userRoles = userRoles;

  constructor(
  ) { }

  ngOnInit() {
  }

  // convenience getter for easy access to form fields
  get f() { return this.userForm.controls; }
}
