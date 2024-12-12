import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
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
import { QuizQuestionComponent } from "../../components/quiz-question/quiz-question.component";
import { FormsModule } from '@angular/forms';
import { SpinnerLoadingComponent } from "../../components/spinner-loading/spinner-loading.component";
import { ErrorUtils } from '../../utils/error-utils';

@Component({
  selector: 'app-quiz',
  standalone: true,
  imports: [CommonModule, PlayQuizButtonComponent, DeleteQuizComponent, BackIconComponent, RouterLink, QuizQuestionComponent, FormsModule, SpinnerLoadingComponent],
  templateUrl: './quiz.component.html',
  styleUrl: './quiz.component.css'
})
export class QuizComponent implements OnInit {
  id!: string;
  quiz: Quiz | null = null; 
  questions: Question[] = [];
  account: Account | null = null;
  isLoadingQuiz = true;
  isUpdating = false;

  constructor (private route: ActivatedRoute, private quizService: QuizService, private toastService: ToastService, private accountService: AccountService, private location: Location, private confirmationToastService: ConfirmationToastService, private questionService: QuestionService) {}

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id') || '';
    this.setQuiz();
    this.setQuizQuestions();
    this.setAccount();
  }

  private setQuiz() {
    this.isLoadingQuiz = true;
    this.quizService.getQuizById(this.id).subscribe({
      next: (response: any) => {
        this.quiz = response.data;
        this.isLoadingQuiz = false;
      },
      error: () => {
        this.isLoadingQuiz = false;
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

  handleUpdateQuiz() {
    if (!this.quiz)
      return

    this.isUpdating = true;
    this.quizService.updateQuiz(
      this.quiz.title, this.quiz.description, this.quiz.expires, this.quiz.expiresInSeconds, this.quiz.difficulty, this.quiz.theme, this.quiz.id).subscribe({
        next: () => {
          this.toastService.showToast('Quiz atualizado com sucesso!', true)
          this.isUpdating = false;
        },
        error: (error) => {
          console.log(error)
          this.isUpdating = false;
          this.toastService.showToast(ErrorUtils.getErrorFromResponse(error, "Ocorreu um erro ao atualizar. Tente novamente mais tarde"), false)
        }
      })
  }

  toggle() {
    this.confirmationToastService.showToast(`Quer ${this.quiz!.isActive ? 'desativar' : 'ativar'} o quiz?`);
    this.confirmationToastService.confirmed$.pipe(take(1)).subscribe({
      next: (confirm) => {
        if (confirm){
          this.quizService.toggleQuiz(this.quiz!.id).subscribe({
            next: () => {
              this.quiz!.isActive = !this.quiz!.isActive;
              this.toastService.showToast(`Quiz ${this.quiz!.isActive ? 'ativado' : 'desativado'} com sucesso!`, true)
            },
            error: (error) => {
              this.toastService.showToast(ErrorUtils.getErrorFromResponse(error), false)
            }
          });
        }
      }
    })
  }

  formatDifficulty(difficulty: string) {
    return QuizUtils.FormatDifficulty(difficulty);
  }

  removeQuestion(questionId: string) {
    const question = this.questions.find(x => x.id === questionId);
    this.questions = this.questions.filter(x => x.id !== questionId);
    if (!question)
      return
    //Reajustar ordem das questões 
    this.questions.forEach(q => {
      if (q.order > question.order)
        q.order--;
    })
  }

  isOwner() {
    if (this.quiz && this.account)
      return this.quiz.userId === this.account.id;
    return false;
  }
}
