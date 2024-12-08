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
import { BackIconComponent } from "../../components/back-icon/back-icon.component";

@Component({
  selector: 'app-match',
  standalone: true,
  imports: [CommonModule, RouterLink, DeleteMatchComponent, BackIconComponent],
  templateUrl: './match.component.html',
  styleUrl: './match.component.css'
})
export class MatchDetailsComponent {
  id!: string;
  match: Match | null = null; 
  account: Account | null = null;

  constructor (private route: ActivatedRoute, private toastService: ToastService, private accountService: AccountService, private matchService: MatchService, private location: Location) {}

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id') || '';
    this.setAccount();
    this.setMatch();
  }

  private setMatch() {
    this.matchService.getMatchById(this.id).subscribe({
      next: (response: any) => {
        this.match = response.data;
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

  format(date: Date) {
    return DateUtils.FormatDate(date);
  }
}
