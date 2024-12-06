import { Component, OnInit } from '@angular/core';
import { PresentationBoxComponent } from "../../components/presentation-box/presentation-box.component";
import { Quiz } from '../../interfaces/Quiz';
import { AccountService } from '../../services/account/account.service';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Match } from '../../interfaces/Match';
import { DateUtils } from '../../utils/date-utils';
import Account from '../../interfaces/Account';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [PresentationBoxComponent, RouterLink, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  quizzes: Quiz[] = []
  matches: Match[] = [];
  account: Account | null = null;

  constructor (private accountService: AccountService) {}

  ngOnInit(): void {
    this.accountService.getAccountMatches();
    this.accountService.getAccountQuizzes();
    this.setAccount();

    this.accountService.$quizzes.subscribe({
      next: (data) => {
        this.quizzes = data;
      }
    })

    this.accountService.$matches.subscribe({
      next: (data) => {
        this.matches = data;
      }
    })
  }

  formatDate(date: Date) {
    return DateUtils.FormatDate(date);
  }

  setAccount() {
    this.accountService.$user.subscribe({
      next: (data) => {
        this.account = data;
      }
    })
  }
  
}
