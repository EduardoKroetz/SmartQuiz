import { CommonModule } from '@angular/common';
import { Component} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { QuizService } from '../../services/quiz/quiz.service';
import { ToastService } from '../../services/toast/toast.service';
import { CreateQuiz, createQuizErrorDefault, CreateQuizErrors } from '../../interfaces/Quiz';
import { ErrorUtils } from '../../utils/error-utils';
import { Router } from '@angular/router';
import { BackIconComponent } from "../../components/back-icon/back-icon.component";
import { AccountService } from '../../services/account/account.service';

@Component({
  selector: 'app-create-quiz',
  standalone: true,
  imports: [FormsModule, CommonModule, BackIconComponent],
  templateUrl: './create-quiz.component.html',
  styleUrl: './create-quiz.component.css'
})
export class CreateQuizComponent {
  createQuizProps : CreateQuiz = { title: "", theme: "", description: "", difficulty: "", expires: true, expiresInSeconds: 0, numberOfQuestions: 0 };
  createQuizError : CreateQuizErrors = createQuizErrorDefault;

  constructor (private quizService: QuizService, private accountService: AccountService, private toastService: ToastService, private router: Router) {}

  createQuiz() {
    this.createQuizError = createQuizErrorDefault;
    this.quizService.createQuiz(this.createQuizProps).subscribe({
      next: (response: any) => {
        const quizId = response.data.id;
        this.toastService.showToast("Quiz criado com sucesso!", true);
        this.quizService.getQuizById(quizId).subscribe({
          next: (quizResponse: any) => {
            this.accountService.addQuiz(quizResponse.data);
          }
        })
        this.router.navigate([`/quizzes/${quizId}/questions/0`])
      },
      error: (error) => {
        const errors = error.error.errors;

        this.createQuizError.titleError = ErrorUtils.getErrorMessage(errors, ['Título', 'título']);
        this.createQuizError.descriptionError = ErrorUtils.getErrorMessage(errors, ['Descrição', 'descrição']);
        this.createQuizError.themeError = ErrorUtils.getErrorMessage(errors, ['Tema', 'tema']);
        this.createQuizError.difficultyError = ErrorUtils.getErrorMessage(errors, ['Dificuldade', 'dificuldade']);
        this.createQuizError.expiresInSecondsError = ErrorUtils.getErrorMessage(errors, ['Expiração', 'expiração']);

        if (!this.createQuizError.titleError && !this.createQuizError.descriptionError && !this.createQuizError.themeError && !this.createQuizError.difficultyError && !this.createQuizError.expiresInSecondsError) {
          this.toastService.showToast(errors[0], false);
        }
      }
    })
  }

}
