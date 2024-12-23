import { Component, Input } from '@angular/core';
import { MatchService } from '../../services/match/match.service';
import { ToastService } from '../../services/toast/toast.service';
import { Router } from '@angular/router';
import { AccountService } from '../../services/account/account.service';
import { CommonModule } from '@angular/common';
import { SpinnerLoadingComponent } from "../spinner-loading/spinner-loading.component";
import { ErrorUtils } from '../../utils/error-utils';

@Component({
  selector: 'app-play-quiz-button',
  standalone: true,
  imports: [CommonModule, SpinnerLoadingComponent],
  templateUrl: './play-quiz-button.component.html',
  styleUrl: './play-quiz-button.component.css'
})
export class PlayQuizButtonComponent {
  @Input() quizId = "";

  isCreating = false;

  constructor (private matchService: MatchService, private accountService: AccountService, private toastService: ToastService, private router: Router) {}

  createMatch() {
    this.isCreating = true;
    this.matchService.createMatch(this.quizId).subscribe({
      next: (response: any) => {
        const matchId = response.data.matchId;
        this.matchService.getMatchById(matchId).subscribe({
          next: (response: any) => {
            this.accountService.addMatch(response.data);
          }
        })
        this.isCreating = false;
        this.toastService.showToast("Partida iniciada!", true);
        this.router.navigate(['/matches/play/'+ matchId])
      },
      error: (error) => {
        console.log(error)
        this.isCreating = false;
        this.toastService.showToast(ErrorUtils.getErrorFromResponse(error));
      }
    });
  }
}
