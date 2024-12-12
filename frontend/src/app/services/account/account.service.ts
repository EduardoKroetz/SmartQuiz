import { Injectable } from '@angular/core';
import { ApiService } from '../api/api.service';
import { BehaviorSubject } from 'rxjs';
import { ToastService } from '../toast/toast.service';
import { Quiz } from '../../interfaces/Quiz';
import { Match } from '../../interfaces/Match';
import Account from '../../interfaces/Account';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {  
  private accountSubject = new BehaviorSubject<Account | null>(null);
  private accountQuizzesSubject = new BehaviorSubject<Quiz[]>([]);
  private accountMatchesSubject = new BehaviorSubject<Match[]>([]);

  $user = this.accountSubject.asObservable();
  $quizzes = this.accountQuizzesSubject.asObservable();
  $matches = this.accountMatchesSubject.asObservable();

  firstQuizzesLoad = true;
  firstMatchesLoad = true;
  matchesPageSize = 10;
  matchesPageNumber = 1;
  isMaxMatches = false;

  isLoadingAccount = true;
  isLoadingMatches = true;
  isLoadingQuizzes = true;

  constructor (private apiService: ApiService, private toastService: ToastService, private router: Router) {
    this.setUser();
  }

  setUser() {
    this.isLoadingAccount = true;
    this.apiService.get("accounts").subscribe({
      next: (response: any) => {
        this.accountSubject.next(response.data);
        this.isLoadingAccount = false;
      },
      error: () => {
        this.isLoadingAccount = false;
        this.toastService.showToast("Você não está autenticado! Redirecionando...", );
        setTimeout(() => {
          this.router.navigate(["/login"])
        }, 1500);
      } 
    });
  }

  getAccountQuizzes() {
    if (!this.firstQuizzesLoad)
      return

    this.isLoadingQuizzes = true;
    this.$user.subscribe({
      next: (user) => {
        if (!user)
          return
        this.apiService.get(`accounts/${user.id}/quizzes`).subscribe({
          next: (response: any) => {
            this.accountQuizzesSubject.next(response.data)
            this.firstQuizzesLoad = false;
            this.isLoadingQuizzes = false;
          },
          error: () => {
            this.isLoadingQuizzes = false;
            this.toastService.showToast("Não foi possível obter os quizzes", false);
          }
        })
      }
    })
  }

  getAccountMatches() {
    this.isLoadingMatches = true;
    this.$user.subscribe({
      next: (user) => {
        if (!user)
          return

        this.apiService.get(`matches?pageNumber=${this.matchesPageNumber}&pageSize=${this.matchesPageSize}`).subscribe({
          next: (response: any) => {
            if (response.data.length < this.matchesPageSize)
              this.isMaxMatches = true;
            const matches = this.accountMatchesSubject.getValue();
            this.accountMatchesSubject.next([...matches ,...response.data]);
            this.firstMatchesLoad = false;
            this.isLoadingMatches = false;
          },
          error: () => {
            this.isLoadingMatches = false;
            this.toastService.showToast("Não foi possível obter as partidas", false);
          }
        })
      }
    })
  }

  loadMoreMatches() {
    this.matchesPageNumber++;
    this.getAccountMatches();
  }

  update(username: string, email: string) {
    return this.apiService.put("accounts", { username, email })
  }

  updatePassword(currentPassword: string, newPassword: string) {
    return this.apiService.patch("accounts/password", { currentPassword, newPassword })
  }

  removeMatch(matchId: string) {
    const matches = this.accountMatchesSubject.getValue();
    const newMatches = matches.filter(x => x.id !== matchId);
    this.accountMatchesSubject.next(newMatches);
  }

  removeQuiz(quizId: string) {
    const quizzes = this.accountQuizzesSubject.getValue();
    const newQuizzes = quizzes.filter(x => x.id !== quizId);
    this.accountQuizzesSubject.next(newQuizzes);
  }

  addQuiz(quiz: Quiz) {
    const quizzes = this.accountQuizzesSubject.getValue();
    this.accountQuizzesSubject.next([...quizzes,quiz]);
  }

  addMatch(match: Match) {
    const matches = this.accountMatchesSubject.getValue();
    this.accountMatchesSubject.next([...matches, match]);
  }
}