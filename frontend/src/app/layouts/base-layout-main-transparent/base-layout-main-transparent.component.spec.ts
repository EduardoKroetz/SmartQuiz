import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BaseLayoutMainTransparentComponent } from './base-layout-main-transparent.component';

describe('BaseLayoutMainTransparentComponent', () => {
  let component: BaseLayoutMainTransparentComponent;
  let fixture: ComponentFixture<BaseLayoutMainTransparentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BaseLayoutMainTransparentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BaseLayoutMainTransparentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
