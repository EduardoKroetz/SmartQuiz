import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BackIconComponent } from "../../components/back-icon/back-icon.component";
import { AccountService } from '../../services/account/account.service';
import { QuizService } from '../../services/quiz/quiz.service';
import { ToastService } from '../../services/toast/toast.service';
import { Router } from '@angular/router';
import { SpinnerLoadingComponent } from "../../components/spinner-loading/spinner-loading.component";
import { ErrorUtils } from '../../utils/error-utils';

@Component({
  selector: 'app-generate-quiz',
  standalone: true,
  imports: [CommonModule, FormsModule, BackIconComponent, SpinnerLoadingComponent],
  templateUrl: './generate-quiz.component.html',
  styleUrl: './generate-quiz.component.css'
})
export class GenerateQuizComponent {
  theme = "";
  difficulty = "medium";
  numberOfQuestions = 0;
  expires = true;
  expiresInSeconds = 0;

  isGenerating = false;

  constructor (private accountService: AccountService, private quizService: QuizService, private toastService: ToastService, private route: Router) {}

  handleSubmit() {
    this.isGenerating = true;
    this.quizService.generateQuiz(this.theme, this.difficulty, this.numberOfQuestions, this.expires, this.expiresInSeconds).subscribe({
      next: (response: any) => {
        this.isGenerating = false;
        const quizId = response.data.id;
        this.toastService.showToast("Quiz gerado com sucesso!", true);
        //Buscar o quiz criado para colocar atualizar o estado
        this.quizService.getQuizById(quizId).subscribe({
          next: (response: any) => {
            this.accountService.addQuiz(response.data.quiz)
          },
          error: () => {
            this.toastService.showToast('Não foi possível obter o quiz gerado');
          }
        })
        this.route.navigate(['quizzes/'+quizId])
      },
      error: (error) => {
        this.isGenerating = false;
        this.toastService.showToast(ErrorUtils.getErrorFromResponse(error));
      }
    })
  }
}
