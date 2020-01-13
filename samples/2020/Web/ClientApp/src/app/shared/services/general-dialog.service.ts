import { Injectable } from '@angular/core';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { GeneralDialogComponent } from '../components/general-dialog/general-dialog.component';

@Injectable({
    providedIn: 'root'
})
export class GeneralDialogService {

    constructor(private modalService: NgbModal) { }

    open(title: string, message: string, urlcallback: string): Promise<boolean> {
        var modalRef = this.modalService.open(GeneralDialogComponent, { centered: true });
        modalRef.componentInstance.title = title;
        modalRef.componentInstance.message = message;
        modalRef.componentInstance.urlcallback = urlcallback;

        return modalRef.result;
    }
}
