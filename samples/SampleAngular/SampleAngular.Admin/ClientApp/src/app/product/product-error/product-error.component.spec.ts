import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductErrorComponent } from './product-error.component';

describe('ProductErrorComponent', () => {
  let component: ProductErrorComponent;
  let fixture: ComponentFixture<ProductErrorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductErrorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductErrorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
