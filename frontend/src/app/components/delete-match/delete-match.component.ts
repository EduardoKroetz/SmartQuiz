import { Component, Input } from '@angular/core';
import { AccountService } from '../../services/account/account.service';
import { ToastService } from '../../services/toast/toast.service';
import { MatchService } from '../../services/match/match.service';
import { ConfirmationToastService } from '../../services/confirmation-toast/confirmation-toast.service';

@Component({
  selector: 'app-delete-match',
  standalone: true,
  imports: [],
  templateUrl: './delete-match.component.html',
  styleUrl: './delete-match.component.css'
})
export class DeleteMatchComponent {
  @Input() matchId = '';

  constructor (public accountService: AccountService, private matchService: MatchService, private confirmationToastService: ConfirmationToastService, private toastService: ToastService) {}

  deleteMatch() {
    this.confirmationToastService.showToast("Deseja excluir a partida?");
    const subscription = this.confirmationToastService.confirmed$.subscribe({
      next: (confirm) => {
        if (confirm) {
          this.matchService.deleteMatch(this.matchId).subscribe({
            next: () => {
              this.toastService.showToast("Partida deletada com sucesso!", true);
              this.accountService.removeMatch(this.matchId)
            },
            error: (error) => {
              this.toastService.showToast(error.error.errors[0], false);
            }
          });
        }
        subscription.unsubscribe();
      }
    })
  }
}
