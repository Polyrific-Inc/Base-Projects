import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { EventService } from '@app/core/services/event.service';
import { EventDto } from '@app/core/models/event/event-dto';
import { EventReservationService } from '@app/core/services/eventreservation.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-myevent-popup',
  templateUrl: './myevent-popup.component.html',
  styleUrls: ['./myevent-popup.component.scss']
})
export class MyeventPopupComponent implements OnInit {
  public isDarkThemeSelected = false;
  events: EventDto[];
  private unregisteredEvents: number[];
  constructor(public activeModal: NgbActiveModal, public eventService: EventService, public eventReservationService: EventReservationService,private toastrService: ToastrService) { }

  ngOnInit() {
    this.getEvents();
    this.unregisteredEvents = [];
  }

  getEvents() {
    this.eventService.getEventsByReserverId().subscribe(data => {
      this.events = data;
    });
  }

  registerChange(event: any, id: number){
    if(event.target.checked === false){
      this.unregisteredEvents.push(id);
    }
    else{
      if(this.unregisteredEvents.length > 0){
        this.unregisteredEvents.splice(this.unregisteredEvents.indexOf(id), 1);
      }
    }
  }

  saveButtonClick(){
    if(this.unregisteredEvents.length > 0){
    this.eventReservationService.deleteMultipleEventReservation(this.unregisteredEvents).subscribe(()=>{
      this.activeModal.close();
      this.toastrService.success("You have been unregistered from selected Events ","Success")
    });
    }
  }

}
