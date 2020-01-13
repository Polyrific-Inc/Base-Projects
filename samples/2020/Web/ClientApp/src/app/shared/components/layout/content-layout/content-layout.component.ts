import { Component, OnInit, AfterViewInit, HostListener } from '@angular/core';
import { trigger, transition, useAnimation } from '@angular/animations';
import { bounce, zoomOut, zoomIn, fadeIn, bounceIn } from 'ng-animate';
import { NavService } from '../../../services/nav.service';
import * as feather from 'feather-icons';
import { CustomizerService } from '@app/shared/services/customize.servicer';

@Component({
  selector: 'app-content-layout',
  templateUrl: './content-layout.component.html',
  styleUrls: ['./content-layout.component.scss'],
  animations: [
    trigger('animateRoute', [transition('* => *', useAnimation(fadeIn, {
      // Set the duration to 5seconds and delay to 2 seconds
      //params: { timing: 3}
    }))])
  ]
})
export class ContentLayoutComponent implements OnInit, AfterViewInit {


  public right_side_bar: boolean;

  constructor(public navServices: NavService, public customizer: CustomizerService) { }


  ngAfterViewInit() {
    setTimeout(() => {
      feather.replace();
    });
  }

  @HostListener('document:click', ['$event'])
  clickedOutside(event) {
    // click outside Area perform following action
    document.getElementById('outer-container').onclick = function (e) {
      e.stopPropagation()
      if (e.target != document.getElementById('search-outer')) {
        document.getElementsByTagName("body")[0].classList.remove("offcanvas");
      }
      if (e.target != document.getElementById('outer-container') && document.getElementById("canvas-bookmark") != null) {
        document.getElementById("canvas-bookmark").classList.remove("offcanvas-bookmark");
      }

      if(e.target != document.getElementsByClassName('Typeahead-menu')[0]){
          var container = <HTMLElement>document.getElementsByClassName('Typeahead-menu is-open')[0];
          if(container != undefined){
            if(!container.classList.contains('loader'))
            {
              container.classList.remove('is-open');
            }      
          }
            
      }
    }

    var navService = this.navServices;
    var pageBodyElem = <HTMLElement>document.getElementsByClassName('page-body')[0];
    pageBodyElem.onclick = function (e) {
        document.getElementById("sidebar-toggle-expanded").style.display = 'none';
        document.getElementById("sidebar-toggle").style.display = 'block';
        navService.collapseSidebar = true;
    }

    var pageBodyElem = <HTMLElement>document.getElementsByClassName('footer')[0];
    pageBodyElem.onclick = function (e) {
        document.getElementById("sidebar-toggle-expanded").style.display = 'none';
        document.getElementById("sidebar-toggle").style.display = 'block';
        navService.collapseSidebar = true;
    }

   
  }

  public getRouterOutletState(outlet) {
    return outlet.isActivated ? outlet.activatedRoute : '';
  }

  public rightSidebar($event) {
    this.right_side_bar = $event
  }

  ngOnInit() { }
}
