import { Component, OnInit } from '@angular/core';
import { UserDto } from '@app/core/models/user/user-dto';
import { ConfirmationDialogService } from '@app/shared/services/confirmation-dialog.service';
import { UserService } from '@app/core/services/user.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {

  users: UserDto[];

  constructor(private userService: UserService, private dialog: ConfirmationDialogService) { }

  ngOnInit() {
    this.getUsers();
  }

  onDeleteClicked(userId: number) {
    this.dialog.open('Confirm Delete User', `Are you sure you want to delete user ${userId}?`).then(result => {
      if (result) {
        this.userService.deleteUser(userId).subscribe(() => {
          this.getUsers();
        });
      }
    }).catch(() => console.log('dismissed'));
  }

  getUsers() {
    this.userService.getUsers().subscribe(data => this.users = data);
  }

}
