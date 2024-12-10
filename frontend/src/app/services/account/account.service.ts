import { Injectable } from '@angular/core';
import { ApiService } from '../api/api.service';
import { BehaviorSubject } from 'rxjs';
import { ToastService } from '../toast/toast.service';
import { Quiz } from '../../interfaces/Quiz';
import { Match } from '../../interfaces/Match';
import Account from '../../interfaces/Account';

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

  constructor (private apiService: ApiService, private toastService: ToastService) {
    this.setUser();
  }

  setUser() {
    this.apiService.get("accounts").subscribe({
      next: (response: any) => {
        this.accountSubject.next(response.data);
      },
      error: () => {
        this.toastService.showToast("Occoreu um erro ao tentar buscar as informações da conta", );
      } 
    });
  }

  getAccountQuizzes() {
    if (!this.firstQuizzesLoad)
      return

    this.$user.subscribe({
      next: (user) => {
        if (!user)
          return
        this.apiService.get(`accounts/${user.id}/quizzes`).subscribe({
          next: (response: any) => {
            this.accountQuizzesSubject.next(response.data)
            this.firstQuizzesLoad = false;
          },
          error: () => {
            this.toastService.showToast("Não foi possível obter os quizzes", false);
          }
        })
      }
    })
  }

  getAccountMatches() {
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
          },
          error: () => {
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