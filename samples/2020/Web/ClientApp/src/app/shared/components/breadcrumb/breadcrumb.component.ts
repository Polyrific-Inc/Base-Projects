import { Component, OnInit } from '@angular/core';
import { Router,RoutesRecognized, ActivatedRouteSnapshot, ActivationEnd, NavigationEnd } from '@angular/router';

import { filter } from 'rxjs/operators';
import { map } from 'rxjs/internal/operators';

@Component({
    selector: 'app-breadcrumb',
    templateUrl: './breadcrumb.component.html',
    styleUrls: ['./breadcrumb.component.scss']
})
export class BreadcrumbComponent implements OnInit {

    public breadcrumbs;
    public title: string;
    public showBreadcumb: boolean;

    constructor(private router: Router) {
        this.router.events.pipe(
            filter(event => event instanceof ActivationEnd),
        )
        .subscribe(eventData => {
            this.showBreadcumb = true;
            let event: any = eventData;
            let currentUrlPart = event.snapshot.root;
            let currUrl = ''; //for HashLocationStrategy

            this.breadcrumbs = [];
            while (currentUrlPart.children.length > 0) {
                currentUrlPart = currentUrlPart.children[0];

                currUrl += '/' + currentUrlPart.url.map(function (item) {
                    return item.path;
                }).join('/');

                if (this.breadcrumbs.find(x => x.url == currUrl.replace(/\/$/, "")) == undefined) {
                    this.breadcrumbs.push({
                        displayName: (<any>currentUrlPart.data).breadcrumb,
                        url: currUrl,
                        params: currentUrlPart.params
                    })
                }
                this.title = (<any>currentUrlPart.data).title;
            }
            if (this.title == "Dashboard")
                this.showBreadcumb = false;
        });
    }

    ngOnInit() { }

}
