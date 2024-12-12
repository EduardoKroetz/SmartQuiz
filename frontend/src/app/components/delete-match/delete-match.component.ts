import { Component, Input } from '@angular/core';
import { AccountService } from '../../services/account/account.service';
import { ToastService } from '../../services/toast/toast.service';
import { MatchService } from '../../services/match/match.service';
import { ConfirmationToastService } from '../../services/confirmation-toast/confirmation-toast.service';
import { Router } from '@angular/router';
import { ErrorUtils } from '../../utils/error-utils';
import { CommonModule } from '@angular/common';
import { SpinnerLoadingComponent } from "../spinner-loading/spinner-loading.component";

@Component({
  selector: 'app-delete-match',
  standalone: true,
  imports: [CommonModule, SpinnerLoadingComponent],
  templateUrl: './delete-match.component.html',
  styleUrl: './delete-match.component.css'
})
export class DeleteMatchComponent {
  @Input() matchId = '';

  isDeleting = false;

  constructor (public accountService: AccountService, private matchService: MatchService, private confirmationToastService: ConfirmationToastService, private toastService: ToastService, private router: Router) {}

  deleteMatch() {
    this.confirmationToastService.showToast("Deseja excluir a partida?");
    const subscription = this.confirmationToastService.confirmed$.subscribe({
      next: (confirm) => {
        if (confirm) {
          this.isDeleting = true;
          this.matchService.deleteMatch(this.matchId).subscribe({
            next: () => {
              this.isDeleting = false;
              this.toastService.showToast("Partida deletada com sucesso!", true);
              this.accountService.removeMatch(this.matchId)
              this.router.navigate(['/history']);
            },
            error: (error) => {
              this.isDeleting = false;
              this.toastService.showToast(ErrorUtils.getErrorFromResponse(error), false);
            }
          });
        }
        subscription.unsubscribe();
      }
    })
  }
}
