import { Injectable } from '@angular/core';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationDialogComponent } from '../components/confirmation-dialog/confirmation-dialog.component';

@Injectable({
    providedIn: 'root'
})
export class ConfirmationDialogService {

    constructor(private modalService: NgbModal) { }

    open(title: string, message: string): Promise<boolean> {
        var modalRef = this.modalService.open(ConfirmationDialogComponent, { centered: true });
        modalRef.componentInstance.title = title;
        modalRef.componentInstance.message = message;

        return modalRef.result;
    }
}
