import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LayoutNoMainComponent } from './layout-no-main.component';

describe('LayoutNoMainComponent', () => {
  let component: LayoutNoMainComponent;
  let fixture: ComponentFixture<LayoutNoMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LayoutNoMainComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LayoutNoMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
