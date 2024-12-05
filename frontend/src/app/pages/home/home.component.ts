import { Component, OnInit } from '@angular/core';
import { PresentationBoxComponent } from "../../components/presentation-box/presentation-box.component";
import { Quiz } from '../../interfaces/Quiz';
import { AccountService } from '../../services/account/account.service';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Match } from '../../interfaces/Match';
import { DateUtils } from '../../utils/date-utils';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [PresentationBoxComponent, RouterLink, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  maxScore = 0;
  minTime = 0;
  matchesPlayed = 0;
  totalScore = 0;
  correctAnswers = 0;
  createdQuizzes = 0;

  quizzes: Quiz[] = []
  matches: Match[] = [];

  constructor (private accountService: AccountService) {}

  ngOnInit(): void {
    this.accountService.getAccountMatches();
    this.accountService.getAccountQuizzes();

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

  
}
