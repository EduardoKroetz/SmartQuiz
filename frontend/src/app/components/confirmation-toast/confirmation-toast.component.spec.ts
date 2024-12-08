import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmationToastComponent } from './confirmation-toast.component';

describe('ConfirmationToastComponent', () => {
  let component: ConfirmationToastComponent;
  let fixture: ComponentFixture<ConfirmationToastComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConfirmationToastComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConfirmationToastComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
