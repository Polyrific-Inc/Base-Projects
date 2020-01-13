import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { NavService, Menu } from '../../services/nav.service';
import { AuthService } from '@app/core/auth/auth.service';
import { Router } from '@angular/router';

const body = document.getElementsByTagName('body')[0];

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {

  public menuItems: Menu[];
  public items: Menu[];
  public searchResult = false;
  public searchResultEmpty = false;
  public openNav = false;
  public right_sidebar = false;
  public text: string;
  public isOpenMobile = false;
  @Output() rightSidebarEvent = new EventEmitter<boolean>();

  constructor(public navServices: NavService,
    public authService: AuthService,
    private router: Router) {
  }


  ngOnDestroy() {
    this.removeFix();
  }


  right_side_bar() {
    this.right_sidebar = !this.right_sidebar;
    this.rightSidebarEvent.emit(this.right_sidebar);
  }

  collapseSidebar() {
    this.navServices.collapseSidebar = !this.navServices.collapseSidebar;
  }

  openMobileNav() {
    this.openNav = !this.openNav;
  }

  searchTerm(term: any) {
    term ? this.addFix() : this.removeFix();
    if (!term) {
      return this.menuItems = [];
    }
    const items = [];
    term = term.toLowerCase();
    this.items.filter(menuItems => {
      if (menuItems.title.toLowerCase().includes(term) && menuItems.type === 'link') {
        items.push(menuItems);
      }
      if (!menuItems.children) {
        return false;
      }
      menuItems.children.filter(subItems => {
        if (subItems.title.toLowerCase().includes(term) && subItems.type === 'link') {
          subItems.icon = menuItems.icon;
          items.push(subItems);
        }
        if (!subItems.children) {
          return false;
        }

        subItems.children.filter(suSubItems => {
          if (suSubItems.title.toLowerCase().includes(term)) {
            suSubItems.icon = menuItems.icon;
            items.push(suSubItems);
          }
        });
      });
      this.checkSearchResultEmpty(items);
      this.menuItems = items;
    });
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
  ngOnInit() {
    this.navServices.items.subscribe(menuItems => {
      this.items = menuItems;
    });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

}
