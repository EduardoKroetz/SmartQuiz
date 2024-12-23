import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../../services/account/account.service';
import { Question } from '../../interfaces/Question';
import Account from '../../interfaces/Account';
import { ConfirmationToastService } from '../../services/confirmation-toast/confirmation-toast.service';
import { QuestionService } from '../../services/question/question.service';
import { ToastService } from '../../services/toast/toast.service';
import { take } from 'rxjs';
import { Quiz } from '../../interfaces/Quiz';
import { SpinnerLoadingComponent } from "../spinner-loading/spinner-loading.component";
import { ErrorUtils } from '../../utils/error-utils';

@Component({
  selector: 'app-quiz-question',
  standalone: true,
  imports: [CommonModule, SpinnerLoadingComponent],
  templateUrl: './quiz-question.component.html',
  styleUrl: './quiz-question.component.css'
})
export class QuizQuestionComponent implements OnInit {
  @Output() deleteQuestionEvent = new EventEmitter<string>();
  @Input() question : Question = null!;
  @Input() quiz: Quiz = null!;
  account: Account | null = null;
  detailsOpen = false;
  isDeleting = false;

  constructor (private accountService: AccountService, private confirmationToastService: ConfirmationToastService, private questionService: QuestionService, private toastService: ToastService) {}

  ngOnInit(): void {
    this.accountService.$user.subscribe({
      next: (data) => {
        this.account = data;
      }
    })
  }

  toggle() {
    if (this.account?.id === this.quiz.userId)
      this.detailsOpen = !this.detailsOpen;
  }

  deleteQuestion(questionId: string, order: number) {
    this.confirmationToastService.showToast(`Deseja excluir a questão ${order + 1}?`);
    this.confirmationToastService.confirmed$.pipe(take(1)).subscribe({
      next: (confirm) => {
        if (confirm) {
          this.isDeleting = true;
          this.questionService.deleteQuestion(questionId).subscribe({
            next: () => {
              this.toastService.showToast("Questão deletada com sucesso!", true);
              this.isDeleting = false;
              this.deleteQuestionEvent.emit(questionId);
            },
            error: (error) => {
              this.isDeleting = false;
              this.toastService.showToast(ErrorUtils.getErrorFromResponse(error));
            }
          })
        }
      }
    })
  }
}
