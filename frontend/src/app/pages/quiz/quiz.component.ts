import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { QuizService } from '../../services/quiz/quiz.service';
import { Quiz } from '../../interfaces/Quiz';
import { ToastService } from '../../services/toast/toast.service';
import { Question } from '../../interfaces/Question';
import { CommonModule, Location } from '@angular/common';
import { AccountService } from '../../services/account/account.service';
import Account from '../../interfaces/Account';
import { PlayQuizButtonComponent } from "../../components/play-quiz-button/play-quiz-button.component";
import { DeleteQuizComponent } from "../../components/delete-quiz/delete-quiz.component";
import { BackIconComponent } from "../../components/back-icon/back-icon.component";

@Component({
  selector: 'app-quiz',
  standalone: true,
  imports: [CommonModule, PlayQuizButtonComponent, DeleteQuizComponent, BackIconComponent],
  templateUrl: './quiz.component.html',
  styleUrl: './quiz.component.css'
})
export class QuizComponent implements OnInit {
  id!: string;
  quiz: Quiz | null = null; 
  questions: Question[] = [];
  account: Account | null = null;

  constructor (private route: ActivatedRoute, private quizService: QuizService, private toastService: ToastService, private router: Router, private accountService: AccountService, private location: Location) {}

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id') || '';
    this.setQuiz();
    this.setQuizQuestions();
    this.setAccount();
  }

  private setQuiz() {
    this.quizService.getQuizById(this.id).subscribe({
      next: (response: any) => {
        this.quiz = response.data;
      },
      error: () => {
        this.toastService.showToast("Não foi possível obter o quiz", false);
        this.location.back();
      }
    })
  }

  private setQuizQuestions() {
    this.quizService.getQuizQuestions(this.id).subscribe({
      next: (response: any) => {
        this.questions = response.data;
      },
      error: () => {
        this.toastService.showToast("Não foi possível carregar as questões", false);
      }
    })
  }

  private setAccount() {
    this.accountService.$user.subscribe({
      next: (data) => {
        this.account = data;
      }
    })
  }
}
