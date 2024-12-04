import { Component, Input } from '@angular/core';
import { defaultQuiz, Quiz } from '../../interfaces/Quiz';

@Component({
  selector: 'app-quiz-card',
  standalone: true,
  imports: [],
  templateUrl: './quiz-card.component.html',
  styleUrl: './quiz-card.component.css'
})
export class QuizCardComponent {
  @Input() quiz: Quiz = { ...defaultQuiz};

}
