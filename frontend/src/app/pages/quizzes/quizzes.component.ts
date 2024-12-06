import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Quiz } from '../../interfaces/Quiz';
import { CommonModule } from '@angular/common';
import { QuizCardComponent } from "../../components/quiz-card/quiz-card.component";
import { AccountService } from '../../services/account/account.service';

@Component({
  selector: 'app-quizzes',
  standalone: true,
  imports: [RouterLink, CommonModule, QuizCardComponent],
  templateUrl: './quizzes.component.html',
  styleUrl: './quizzes.component.css'
})
export class QuizzesComponent implements OnInit {
  quizzes: Quiz[] = [];

  constructor (private accountService: AccountService) {}

  ngOnInit(): void {
    this.accountService.getAccountQuizzes();
    this.getQuizzes();
  }

  private getQuizzes() {
    this.accountService.$quizzes.subscribe({
      next: (data) => {
        this.quizzes = data;
      }
    })
  }
}
