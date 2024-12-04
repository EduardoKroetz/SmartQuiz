import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BaseLayoutMainComponent } from './base-layout-main.component';

describe('BaseLayoutMainComponent', () => {
  let component: BaseLayoutMainComponent;
  let fixture: ComponentFixture<BaseLayoutMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BaseLayoutMainComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BaseLayoutMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
