import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ConfirmationDialogComponent } from './components/confirmation-dialog/confirmation-dialog.component';
import { GeneralDialogComponent } from './components/general-dialog/general-dialog.component';

import { LoaderComponent } from './components/loader/loader.component';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { ContentLayoutComponent } from './components/layout/content-layout/content-layout.component';
import { FullLayoutComponent } from './components/layout/full-layout/full-layout.component';
import { FeatherIconsComponent } from './components/feather-icons/feather-icons.component';
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';
import { BookmarkComponent } from './components/bookmark/bookmark.component';
import { NgbModalModule, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { DragulaModule } from 'ng2-dragula';

import { NavService } from './services/nav.service';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { WINDOW_PROVIDERS } from "./services/window.service";

// Directives
import { ToggleFullscreenDirective } from './directives/fullscreen.directive';
import { MyeventPopupComponent } from './components/myevent-popup/myevent-popup.component';

@NgModule({
    declarations: [
        ConfirmationDialogComponent,
        GeneralDialogComponent,
        LoaderComponent,
        HeaderComponent,
        FooterComponent,
        SidebarComponent,
        BookmarkComponent,
        ContentLayoutComponent,
        FullLayoutComponent,
        FeatherIconsComponent,
        ToggleFullscreenDirective,
        BreadcrumbComponent,
        MyeventPopupComponent,
    ],
    imports: [
        CommonModule,
        NgbModalModule,
        NgbModule,
        FormsModule,
        DragulaModule.forRoot(),
        RouterModule
    ],
    exports: [
        ConfirmationDialogComponent,
        GeneralDialogComponent,
        FeatherIconsComponent,
        LoaderComponent
    ],
    providers: [
        NavService,
        WINDOW_PROVIDERS
    ],
    entryComponents: [
        ConfirmationDialogComponent,
        MyeventPopupComponent,
        GeneralDialogComponent
    ]
})
export class SharedModule { }
