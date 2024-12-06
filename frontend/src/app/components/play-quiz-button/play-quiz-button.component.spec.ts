import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayQuizButtonComponent } from './play-quiz-button.component';

describe('PlayQuizButtonComponent', () => {
  let component: PlayQuizButtonComponent;
  let fixture: ComponentFixture<PlayQuizButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PlayQuizButtonComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlayQuizButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
