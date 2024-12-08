import { Component, OnInit } from '@angular/core';
import { HistoryItemComponent } from "../../components/history-item/history-item.component";
import { CommonModule } from '@angular/common';
import { AccountService } from '../../services/account/account.service';
import { Match } from '../../interfaces/Match';
import { BackIconComponent } from "../../components/back-icon/back-icon.component";

@Component({
  selector: 'app-history',
  standalone: true,
  imports: [CommonModule, HistoryItemComponent, BackIconComponent],
  templateUrl: './history.component.html',
  styleUrl: './history.component.css'
})
export class HistoryComponent implements OnInit {
  matches: Match[] = [];

  constructor (public accountService: AccountService) {}

  ngOnInit(): void {
    this.accountService.getAccountMatches();
    this.setMatches();
  }

  private setMatches() {
    this.accountService.$matches.subscribe({
      next: (data) => {
        console.log(data)
        this.matches = data;
      }
    })
  }
}
