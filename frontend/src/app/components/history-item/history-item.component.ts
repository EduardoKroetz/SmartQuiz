import { Component, Input, OnInit } from '@angular/core';
import { Match } from '../../interfaces/Match';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { DateUtils } from '../../utils/date-utils';
import { AccountService } from '../../services/account/account.service';
import { ToastService } from '../../services/toast/toast.service';
import { MatchService } from '../../services/match/match.service';
import { ConfirmationToastService } from '../../services/confirmation-toast/confirmation-toast.service';
import { DeleteMatchComponent } from "../delete-match/delete-match.component";
import { MatchUtils } from '../../utils/match-utils';
import { take } from 'rxjs';
import { ErrorUtils } from '../../utils/error-utils';

@Component({
  selector: 'app-history-item',
  standalone: true,
  imports: [CommonModule, RouterLink, DeleteMatchComponent],
  templateUrl: './history-item.component.html',
  styleUrl: './history-item.component.css'
})
export class HistoryItemComponent {
  @Input() match: Match | null = null;

  constructor (public accountService: AccountService, private matchService: MatchService, private confirmationToastService: ConfirmationToastService, private toastService: ToastService) {}

  formatDate(date: Date) {
    return DateUtils.FormatDate(date);
  }

  deleteMatch() {
    this.confirmationToastService.showToast("Deseja excluir a partida?");
    const subscription = this.confirmationToastService.confirmed$.pipe(take(1)).subscribe({
      next: (confirm) => {
        if (confirm) {
          this.matchService.deleteMatch(this.match!.id).subscribe({
            next: () => {
              this.toastService.showToast("Partida deletada com sucesso!", true);
              this.accountService.removeMatch(this.match!.id)
            },
            error: (error) => {
              this.toastService.showToast(ErrorUtils.getErrorFromResponse(error), false);
            }
          });
        }
        subscription.unsubscribe();
      }
    })
  }

  formatStatus(status: string) {
    return MatchUtils.FormatStatus(status);
  } 
}
