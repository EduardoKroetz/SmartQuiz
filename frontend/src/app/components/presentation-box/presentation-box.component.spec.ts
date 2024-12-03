import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PresentationBoxComponent } from './presentation-box.component';

describe('PresentationBoxComponent', () => {
  let component: PresentationBoxComponent;
  let fixture: ComponentFixture<PresentationBoxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PresentationBoxComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PresentationBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
