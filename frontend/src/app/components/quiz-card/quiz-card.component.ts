import { Component, Input } from '@angular/core';
import { defaultQuiz, Quiz } from '../../interfaces/Quiz';
import { MatchService } from '../../services/match/match.service';
import { ToastService } from '../../services/toast/toast.service';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ErrorUtils } from '../../utils/error-utils';
import { PlayQuizButtonComponent } from "../play-quiz-button/play-quiz-button.component";

@Component({
  selector: 'app-quiz-card',
  standalone: true,
  imports: [RouterLink, CommonModule, PlayQuizButtonComponent],
  templateUrl: './quiz-card.component.html',
  styleUrl: './quiz-card.component.css'
})
export class QuizCardComponent {
  @Input() quiz: Quiz = { ...defaultQuiz};
}
