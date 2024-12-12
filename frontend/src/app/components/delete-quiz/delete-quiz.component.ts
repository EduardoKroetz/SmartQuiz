import { Component, Input } from '@angular/core';
import { AccountService } from '../../services/account/account.service';
import { ConfirmationToastService } from '../../services/confirmation-toast/confirmation-toast.service';
import { ToastService } from '../../services/toast/toast.service';
import { QuizService } from '../../services/quiz/quiz.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SpinnerLoadingComponent } from "../spinner-loading/spinner-loading.component";

@Component({
  selector: 'app-delete-quiz',
  standalone: true,
  imports: [CommonModule, SpinnerLoadingComponent],
  templateUrl: './delete-quiz.component.html',
  styleUrl: './delete-quiz.component.css'
})
export class DeleteQuizComponent {
  @Input() quizId = '';

  isDeleting = false;

  constructor (public accountService: AccountService, private quizService: QuizService, private confirmationToastService: ConfirmationToastService, private toastService: ToastService, private router: Router) {}

  deleteMatch() {
    this.confirmationToastService.showToast("Deseja excluir o quiz?");
    const subscription = this.confirmationToastService.confirmed$.subscribe({
      next: (confirm) => {
        if (confirm) {
          this.isDeleting = true;
          this.quizService.deleteQuiz(this.quizId).subscribe({
            next: () => {
              this.toastService.showToast("Quiz deletado com sucesso!", true);
              this.accountService.removeQuiz(this.quizId);
              this.isDeleting = false;
              this.router.navigate(['quizzes']);
            },
            error: (error) => {
              this.isDeleting = false;
              this.toastService.showToast(error.error.errors[0], false);
            }
          });
        }
        subscription.unsubscribe();
      }
    })
  }
}
