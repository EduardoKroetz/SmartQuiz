import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteMatchComponent } from './delete-match.component';

describe('DeleteMatchComponent', () => {
  let component: DeleteMatchComponent;
  let fixture: ComponentFixture<DeleteMatchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeleteMatchComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeleteMatchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
