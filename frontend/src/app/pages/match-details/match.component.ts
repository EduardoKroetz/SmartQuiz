import { Component } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
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

@Component({
  selector: 'app-match',
  standalone: true,
  imports: [CommonModule, RouterLink, DeleteMatchComponent, ResponseItemComponent],
  templateUrl: './match.component.html',
  styleUrl: './match.component.css'
})
export class MatchDetailsComponent {
  id!: string;
  match: Match | null = null; 
  account: Account | null = null;
  responses: Response[] = []; 
  percentageOfHits = 0;

  constructor (private route: ActivatedRoute, private toastService: ToastService, private accountService: AccountService, private matchService: MatchService, private location: Location) {}

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
      },
      error: () => {
        this.toastService.showToast("Não foi possível obter a partida", false);
        this.location.back();
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
      },
      error: (error) => {
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
}
