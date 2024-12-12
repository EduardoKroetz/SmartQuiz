import { Component } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ToastService } from '../../services/toast/toast.service';
import { AccountService } from '../../services/account/account.service';
import { Match } from '../../interfaces/Match';
import Account from '../../interfaces/Account';
import { MatchService } from '../../services/match/match.service';
import { CommonModule, Location } from '@angular/common';
import { DateUtils } from '../../utils/date-utils';
import { DeleteMatchComponent } from "../../components/delete-match/delete-match.component";
import { Response } from '../../interfaces/Response';
import { ResponseItemComponent } from "../../components/response-item/response-item.component";
import { MatchUtils } from '../../utils/match-utils';
import { SpinnerLoadingComponent } from "../../components/spinner-loading/spinner-loading.component";

@Component({
  selector: 'app-match',
  standalone: true,
  imports: [CommonModule, RouterLink, DeleteMatchComponent, ResponseItemComponent, SpinnerLoadingComponent],
  templateUrl: './match.component.html',
  styleUrl: './match.component.css'
})
export class MatchDetailsComponent {
  id!: string;
  match: Match | null = null; 
  account: Account | null = null;
  responses: Response[] = []; 
  percentageOfHits = 0;

  isLoadingMatch = true;
  isLoadingResponses = true;

  constructor (private route: ActivatedRoute, private toastService: ToastService, private accountService: AccountService, private matchService: MatchService, private location: Location, private router: Router) {}

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id') || '';
    this.setAccount();
    this.setMatch();
    this.setResponses();
  }

  private setMatch() {
    this.matchService.getMatchById(this.id).subscribe({
      next: (response: any) => {
        this.match = response.data;
        this.percentageOfHits = parseFloat(((100 / this.match!.quiz!.numberOfQuestion) * this.match!.score).toFixed(2));
        this.isLoadingMatch = false;
      },
      error: () => {
        this.isLoadingMatch = false;
        this.toastService.showToast("Não foi possível obter a partida", false);
        this.router.navigate(["/history"]);
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

  private setResponses() {
    this.matchService.getResponses(this.id).subscribe({
      next: (response: any) => {
        this.responses = response.data;
        this.isLoadingResponses = false;
      },
      error: (error) => {
        this.isLoadingResponses = false;
        this.toastService.showToast(error.error.errors[0])
      }
    })
  }

  format(date: Date) {
    return DateUtils.FormatDate(date);
  }

  formatStatus(status: string) {
    return MatchUtils.FormatStatus(status);
  }

  continueMatch() {
    if (!this.match || this.match.status != 'Created')
      return;

    this.router.navigate([`/matches/play/${this.match.id}`])
  }
}
