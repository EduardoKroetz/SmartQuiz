import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteQuizComponent } from './delete-quiz.component';

describe('DeleteQuizComponent', () => {
  let component: DeleteQuizComponent;
  let fixture: ComponentFixture<DeleteQuizComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeleteQuizComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeleteQuizComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
