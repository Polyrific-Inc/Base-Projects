import { Injectable, HostListener, OnDestroy } from '@angular/core';
import { BehaviorSubject, Observable, Subscriber, Subscription } from 'rxjs';
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
            path: '/directory', title: 'Directory', type: 'link', icon: 'users'
        },
        {
            path: '/news', title: 'News', type: 'link', icon: 'rss'
        },
        {
            path: '/event', title: 'Events', type: 'link', icon: 'calendar'
        },
        {
          path: '/launchpad', title: 'LaunchPad', type: 'link', icon: 'grid', disallowedDevices: ['mobile', 'tablet']
        },
        {
            path: '/gallery', title: 'Photo Gallery', type: 'link', icon: 'image'
        },
        {
            path: '/document', title: 'Documents', type: 'link', icon: 'folder'
        },
        {
            title: 'Admin', icon: 'user-plus', type: 'sub', active: false, children: [
                {
                    path: '/admin/banner', title: 'Home Page Banners', type: 'link', icon: 'users'
                },
                {
                    path: '/admin/quicklink', title: 'QuickLinks', type: 'link', icon: 'users'
                },
                {
                    path: '/admin/notification', title: 'Notifications', type: 'link'
                },
                {
                    path: '/admin/notificationtype', title: 'Notification Types', type: 'link'
                },
                {
                    path: '/admin/newsarticle', title: 'News', type: 'link'
                },
                {
                    path: '/admin/event', title: 'Events', type: 'link'
                },
                {
                    path: '/admin/gallery', title: 'Photo Gallery', icon: 'image', type: 'link'
                },
                {
                    path: '/admin/documentcategory', title: 'Documents', type: 'link', icon: 'folder'
                },
                {
                    path: '/admin/documentgroup', title: 'Document Groups', type: 'link', icon: 'folders'
                },
                {
                    path: '/admin/pushnotification', title: 'Push Notification', type: 'link', icon: 'folder'
                },
            ]
        }];

    MENUITEMSEDITOR: Menu[] = [
        {
            title: 'Home Page', icon: 'home', type: 'link', path: '/'
        },
        {
            path: '/directory', title: 'Directory', type: 'link', icon: 'users'
        },
        {
            path: '/news', title: 'News', type: 'link', icon: 'rss'
        },
        {
            path: '/event', title: 'Events', type: 'link', icon: 'calendar'
        },
        {
            path: '/launchpad', title: 'LaunchPad', type: 'link', icon: 'grid', disallowedDevices: ['mobile', 'tablet']
        },
        {
            path: '/gallery', title: 'Photo Gallery', type: 'link', icon: 'image'
        },
        {
            path: '/document', title: 'Documents', type: 'link', icon: 'folder'
        },
        {
            title: 'Editor', icon: 'edit', type: 'sub', active: false, children: [
                {
                    path: '/admin/banner', title: 'Home Page Banners', type: 'link', icon: 'users'
                },
                {
                    path: '/admin/newsarticle', title: 'News', type: 'link'
                },
                {
                    path: '/admin/event', title: 'Events', type: 'link'
                },
                {
                    path: '/admin/gallery', title: 'Photo Gallery', icon: 'image', type: 'link'
                },
                {
                    path: '/admin/documentcategory', title: 'Documents', type: 'link', icon: 'folder'
                },
                {
                    path: '/admin/documentgroup', title: 'Document Groups', type: 'link', icon: 'folders'
                }
            ]
        }
    ];

    MENUITEMS: Menu[] = [
        {
            title: 'Home Page', icon: 'home', type: 'link', path: '/'
        },
        {
            path: '/directory', title: 'Directory', type: 'link', icon: 'users'
        },
        {
            path: '/news', title: 'News', type: 'link', icon: 'rss'
        },
        {
            path: '/event', title: 'Events', type: 'link', icon: 'calendar'
        },
        {
            path: '/launchpad', title: 'LaunchPad', type: 'link', icon: 'grid', disallowedDevices: ['mobile', 'tablet']
        },
        {
            path: '/gallery', title: 'Photo Gallery', type: 'link', icon: 'image'
        },
        {
            path: '/document', title: 'Documents', type: 'link', icon: 'folder'
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
