import { Component, Input } from '@angular/core';
import { MatchService } from '../../services/match/match.service';
import { ToastService } from '../../services/toast/toast.service';
import { Router } from '@angular/router';
import { AccountService } from '../../services/account/account.service';

@Component({
  selector: 'app-play-quiz-button',
  standalone: true,
  imports: [],
  templateUrl: './play-quiz-button.component.html',
  styleUrl: './play-quiz-button.component.css'
})
export class PlayQuizButtonComponent {
  @Input() quizId = "";

  constructor (private matchService: MatchService, private accountService: AccountService, private toastService: ToastService, private router: Router) {}

  createMatch() {
    this.matchService.createMatch(this.quizId).subscribe({
      next: (response: any) => {
        const matchId = response.data.matchId;
        this.matchService.getMatchById(matchId).subscribe({
          next: (response: any) => {
            this.accountService.addMatch(response.data);
          }
        })
        this.toastService.showToast("Partida iniciada!", true);
        this.router.navigate(['/matches/play/'+ matchId])
      },
      error: (error) => {
        this.toastService.showToast(error.error.errors[0]);
      }
    });
  }
}
