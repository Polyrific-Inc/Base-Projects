import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';

@Component({
    selector: 'app-general-dialog',
    templateUrl: './general-dialog.component.html',
    styleUrls: ['./general-dialog.component.scss']
})
export class GeneralDialogComponent implements OnInit {
    @Input() title: string;
    @Input() message: string;
    @Input() urlcallback: string;

    constructor(private activeModal: NgbActiveModal, private router: Router) { }

    ngOnInit() {
    }

    public accept() {
        this.activeModal.close(true);
        if (this.urlcallback != '') {
            console.log(this.urlcallback);
            this.router.navigateByUrl(this.urlcallback);
        }
    }

    public dismiss() {
        this.activeModal.dismiss();
        if (this.urlcallback != '') {
            this.router.navigateByUrl(this.urlcallback);
        }
    }
}
