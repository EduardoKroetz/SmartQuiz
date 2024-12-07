import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateQuizQuestionsComponent } from './create-quiz-questions.component';

describe('CreateQuizQuestionsComponent', () => {
  let component: CreateQuizQuestionsComponent;
  let fixture: ComponentFixture<CreateQuizQuestionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateQuizQuestionsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateQuizQuestionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
