import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MyeventPopupComponent } from './myevent-popup.component';

describe('MyeventPopupComponent', () => {
  let component: MyeventPopupComponent;
  let fixture: ComponentFixture<MyeventPopupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MyeventPopupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MyeventPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
