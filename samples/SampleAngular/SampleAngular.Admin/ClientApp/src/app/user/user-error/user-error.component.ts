import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-user-error',
  templateUrl: './user-error.component.html',
  styleUrls: ['./user-error.component.css']
})
export class UserErrorComponent implements OnInit {
  userId: number;

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.userId = this.route.snapshot.params.id;
  }

}
