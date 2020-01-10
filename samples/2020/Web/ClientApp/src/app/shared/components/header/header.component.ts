
import { Component, OnInit, Output, EventEmitter, OnDestroy, HostListener  } from '@angular/core';
import { NavService, Menu } from '../../services/nav.service';
import { AuthService } from '@app/core/auth/auth.service';
import { UserNotificationService } from '@app/core/services/usernotification.service';
import { Router } from '@angular/router';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MyeventPopupComponent } from '../myevent-popup/myevent-popup.component';
import { NotificationService } from '@app/core/services/notification.service';
import { fromEventPattern } from 'rxjs';
import { NotificationCustomDto } from '@app/core/models/notification/notification-custom-dto';
import { NotificationFilterDto } from '@app/core/models/notification/notification-filter-dto';
import { ConfigService } from '@app/config/config.service';
import { distinctUntilChanged, debounceTime } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { SearchService } from '@app/core/services/search.service';
import { SearchDto } from '@app/core/models/search/search-dto'
import { searchEntity } from '@app/core/enum/search-entity';

const body = document.getElementsByTagName('body')[0];

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {
    filter: NotificationFilterDto;
    public menuItems: Menu[];
    public items: Menu[];
    public searchResult = false;
    public searchResultEmpty = false;
    public openNav = false;
    public right_sidebar = false;
    public text: string;
    public isOpenMobile = false;
    public isDarkThemeSelected = false;
    public userEmail: string;
    public notificationsCount: number = 0;
    public notifications: NotificationCustomDto[];
    public allNotifications: NotificationCustomDto[];
    private searchQuery = new Subject<string>();
    searchDto: SearchDto;
    isUser: boolean = false;
    isSmartSearch: boolean = false;
    isSearch:boolean = false;
    

    @Output() rightSidebarEvent = new EventEmitter<boolean>();

    constructor(public navServices: NavService,
        public authService: AuthService,
        private router: Router,
        private modalService:NgbModal,
        private notificationService:NotificationService,
        private userNotificationService: UserNotificationService,
        public configService: ConfigService,
        private searchService: SearchService) {
    }

    ngOnInit() {
        this.isUser = !this.authService.isAdmin && !this.authService.isEditor;
        this.filter = {
            includeAdmin: true,
            skip: null,
            take: null
        };

        this.navServices.items.subscribe(menuItems => {
            this.items = menuItems;
        });

        // get checked value from localstorage
        const layoutVersion = localStorage.getItem("layoutVersion");
        this.isDarkThemeSelected = layoutVersion === 'dark-only' ? true : false;

        // collapse the sidebar on init
        this.navServices.collapseSidebar = true;

        //GetNotifications
        this.getNotification();

        this.InitializeSearch();
    }

    

    ngOnDestroy() {
        this.removeFix();
    }

    right_side_bar() {
        this.right_sidebar = !this.right_sidebar;
        this.rightSidebarEvent.emit(this.right_sidebar);
    }

    //This function toggles the sidebar in and out
    //If this.navServices.collapseSidebar == true then the sidebar is expanding
    collapseSidebar() {
        //Toggle nav menu icon
        if (this.navServices.collapseSidebar) {
            document.getElementById("sidebar-toggle-expanded").style.display = 'block';
            document.getElementById("sidebar-toggle").style.display = 'none';
        } else {
          document.getElementById("sidebar-toggle-expanded").style.display = 'none';
          document.getElementById("sidebar-toggle").style.display = 'block';
        }
        
      this.navServices.collapseSidebar = !this.navServices.collapseSidebar;
    }

    openMobileNav() {
        this.openNav = !this.openNav;
    }

    InitializeSearch() {
        this.searchQuery.pipe(
            debounceTime(1000),
            distinctUntilChanged()
        ).subscribe(searchQuery => {
            let query = encodeURIComponent(searchQuery);
            if(query !== ''){
                this.searchService.search(query).subscribe(data => {
                  this.searchDto = data;
                  this.searchResultEmpty = !this.searchDto.Events && !this.searchDto.Users;
                });
            }          
        });
    }

    searchTerm() {
        var searchResultUrl = '/searchresult/' + this.text;
        this.router.navigateByUrl(searchResultUrl);
    }

    onSearchKeyDown(event){
        if(event.key === "Enter"){
            this.searchTerm();
        }
    }

    checkSearchResultEmpty(items) {
        if (!items.length) {
            this.searchResultEmpty = true;
        } else {
            this.searchResultEmpty = false;
        }
    }

    addFix() {
        this.searchResult = true;
        body.classList.add('offcanvas');
    }

    removeFix() {
        this.searchResult = false;
        body.classList.remove('offcanvas');
        this.text = '';
    }

    myEvent() {
        const modalRef = this.modalService.open(MyeventPopupComponent, { size: "xl" as any, centered: true });
    }

    logout() {
        this.authService.logout();
        //this.router.navigate(['/login']);
    }

    themeChange(event: any) {
        if (event.target.checked === true) {
            localStorage.setItem('layoutVersion', 'dark-only');
        }
        else {
            localStorage.setItem('layoutVersion', 'light');
        }
        window.location.reload();
    }

    @HostListener('document:click', ['$event'])
    onClick(event) {
        var navService = this.navServices;
        var pageBodyElem = <HTMLElement>document.getElementsByClassName('nav-right')[0];
        pageBodyElem.onclick = function (e) {
            document.getElementById("sidebar-toggle-expanded").style.display = 'none';
            document.getElementById("sidebar-toggle").style.display = 'block';
            navService.collapseSidebar = true;
        }

        this.searchResult = false;
        this.searchResultEmpty = false;

        //var container = <HTMLElement>document.getElementsByClassName('Typeahead-menu')[0];
        //container.classList.remove('is-open');
    }

    getNotification(){
        this.notificationService.getFilteredNotificationsbyUserId().subscribe(data => {
            this.notifications = data.results.slice(0,3);
            this.allNotifications = data.results;
            this.notificationsCount = data.count;
        });
    }

    onDeleteNotifClick(id:number){
        let userNotification = {
            id:0,
            notificationId:id,
            userId:'userId'
        };

        let notification = this.notifications.find(x=>x.id == id);
        this.notifications.splice(this.notifications.indexOf(notification), 1);
        this.notificationsCount = this.notificationsCount - 1;

        this.userNotificationService.createUserNotification(userNotification).subscribe(data => {
           
        });
    }

    onAllNoficationClick(){
        this.notifications = this.allNotifications;
        document.getElementById("AllNotifications").style.display = 'none';
    }
}
