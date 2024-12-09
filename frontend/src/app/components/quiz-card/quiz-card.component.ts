import { Component, Input } from '@angular/core';
import { defaultQuiz, Quiz } from '../../interfaces/Quiz';
import { MatchService } from '../../services/match/match.service';
import { ToastService } from '../../services/toast/toast.service';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-quiz-card',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './quiz-card.component.html',
  styleUrl: './quiz-card.component.css'
})
export class QuizCardComponent {
  @Input() quiz: Quiz = { ...defaultQuiz};

  constructor (private matchService: MatchService, private toastService: ToastService, private router: Router) {}

  private createMatch() {
    this.matchService.createMatch(this.quiz.id).subscribe({
      next: (response: any) => {
        const matchId = response.data.matchId;
        this.toastService.showToast("Partida iniciada!", true);
        this.router.navigate(['/matches/play/'+ matchId])
      },
      error: (error) => {
        this.toastService.showToast(error.error.errors[0]);
      }
    });
  }

  handleButtonClick() {
    if (this.quiz.isActive) {
      this.createMatch();
    }else {
      this.router.navigate(['/quizzes/' + this.quiz.id])
    }
  }

}
