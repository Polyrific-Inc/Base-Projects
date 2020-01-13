import { Injectable, HostListener, OnDestroy } from '@angular/core';
import { BehaviorSubject, Subscription } from 'rxjs';
import { AuthService } from '../../core/auth/auth.service';

// Menu
export interface Menu {
    path?: string;
    title?: string;
    icon?: string;
    type?: string;
    badgeType?: string;
    badgeValue?: string;
    active?: boolean;
    bookmark?: boolean;
    children?: Menu[];
    disallowedDevices?: string[];
}

@Injectable({
    providedIn: 'root'
})

export class NavService implements OnDestroy {

    public screenWidth: any;
    public collapseSidebar = false;

    MENUITEMSADMIN: Menu[] = [
        {
            title: 'Home Page', icon: 'home', type: 'link', path: '/'
        },
        {
            path: '/product', title: 'Product', type: 'link', icon: 'folder'
        }];

    MENUITEMSEDITOR: Menu[] = [
        {
            title: 'Home Page', icon: 'home', type: 'link', path: '/'
        },
        {
          path: '/product', title: 'Product', type: 'link', icon: 'folder'
        }
    ];

    MENUITEMS: Menu[] = [
        {
            title: 'Home Page', icon: 'home', type: 'link', path: '/'
        },
        {
          path: '/product', title: 'Product', type: 'link', icon: 'folder'
        }
    ];

    isAdmin = false;
    isEditor = false;
    // Array
    items = new BehaviorSubject<Menu[]>([]);

    private userSubscriber: Subscription;
    constructor(private authService: AuthService) {
        this.onResize();
        if (this.screenWidth < 992) {
            this.collapseSidebar = true;
        }

        this.userSubscriber = this.authService.currentUser.subscribe(user => {
            if (user != null) {
                let currentMenuItems = null;
                this.isAdmin = user.role === 'Administrator';
                this.isEditor = user.role === 'Editor';

                currentMenuItems = this.MENUITEMS;

                if (this.isAdmin)
                    currentMenuItems = this.MENUITEMSADMIN;

                if (this.isEditor)
                    currentMenuItems = this.MENUITEMSEDITOR;

                currentMenuItems.forEach(x => x.active = false);
                this.items.next(currentMenuItems);
            }
        });
    }

    ngOnDestroy() {
        if (this.userSubscriber) {
            this.userSubscriber.unsubscribe();
        }
    }

    // Windows width
    @HostListener('window:resize', ['$event'])
    onResize(event?) {
        this.screenWidth = window.innerWidth;
    }




}
