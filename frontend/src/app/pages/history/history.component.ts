import { Component, OnInit } from '@angular/core';
import { HistoryItemComponent } from "../../components/history-item/history-item.component";
import { CommonModule } from '@angular/common';
import { AccountService } from '../../services/account/account.service';
import { Match } from '../../interfaces/Match';
import { BackIconComponent } from "../../components/back-icon/back-icon.component";
import { SpinnerLoadingComponent } from "../../components/spinner-loading/spinner-loading.component";

@Component({
  selector: 'app-history',
  standalone: true,
  imports: [CommonModule, HistoryItemComponent, BackIconComponent, SpinnerLoadingComponent],
  templateUrl: './history.component.html',
  styleUrl: './history.component.css'
})
export class HistoryComponent implements OnInit {
  matches: Match[] = [];


  constructor (public accountService: AccountService) {}

  ngOnInit(): void {
    if (this.accountService.firstMatchesLoad)
      this.accountService.getAccountMatches();
    this.setMatches();
  }

  private setMatches() {
    this.accountService.$matches.subscribe({
      next: (data) => {
        this.matches = data;
      }
    })
  }

  loadMore() {
    this.accountService.loadMoreMatches();
  }
}
