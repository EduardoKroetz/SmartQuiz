import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GenerateQuizComponent } from './generate-quiz.component';

describe('GenerateQuizComponent', () => {
  let component: GenerateQuizComponent;
  let fixture: ComponentFixture<GenerateQuizComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GenerateQuizComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GenerateQuizComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
