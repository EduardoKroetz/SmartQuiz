import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
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
import { ConfirmationToastService } from '../../services/confirmation-toast/confirmation-toast.service';
import { take } from 'rxjs';
import { QuestionService } from '../../services/question/question.service';
import { QuizUtils } from '../../utils/quiz-utils';

@Component({
  selector: 'app-quiz',
  standalone: true,
  imports: [CommonModule, PlayQuizButtonComponent, DeleteQuizComponent, BackIconComponent, RouterLink],
  templateUrl: './quiz.component.html',
  styleUrl: './quiz.component.css'
})
export class QuizComponent implements OnInit {
  id!: string;
  quiz: Quiz | null = null; 
  questions: Question[] = [];
  account: Account | null = null;

  constructor (private route: ActivatedRoute, private quizService: QuizService, private toastService: ToastService, private accountService: AccountService, private location: Location, private confirmationToastService: ConfirmationToastService, private questionService: QuestionService) {}

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

  toggle() {
    this.confirmationToastService.showToast(`Quer ${this.quiz!.isActive ? 'desativar' : 'ativar'} o quiz?`);
    this.confirmationToastService.confirmed$.pipe(take(1)).subscribe({
      next: (confirm) => {
        console.log(confirm);
        if (confirm){
          this.quizService.toggleQuiz(this.quiz!.id).subscribe({
            next: () => {
              this.quiz!.isActive = !this.quiz!.isActive;
              this.toastService.showToast(`Quiz ${this.quiz!.isActive ? 'ativado' : 'desativado'} com sucesso!`, true)
            },
            error: (error) => {
              this.toastService.showToast(error.error.errors[0], false)
            }
          });
        }
      }
    })
  }
  
  deleteQuestion(questionId: string, order: number) {
    this.confirmationToastService.showToast(`Deseja excluir a questão ${order + 1}?`);
    this.confirmationToastService.confirmed$.pipe(take(1)).subscribe({
      next: (confirm) => {
        if (confirm) {
          this.questionService.deleteQuestion(questionId).subscribe({
            next: () => {
              this.toastService.showToast("Questão deletada com sucesso!", true);
              this.questions = this.questions.filter(x => x.id !== questionId);
            },
            error: (error) => {
              this.toastService.showToast(error.error.errors[0]);
            }
          })
        }
      }
    })
  }

  
  formatDifficulty(difficulty: string) {
    return QuizUtils.FormatDifficulty(difficulty);
  }
}
