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

  private firstQuizzesLoad = true;
  private firstMatchesLoad = true;

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

  setToken(token: string) {
    console.log("token setado:" + token)
    localStorage.setItem("auth-token", token)
  }

  loginAsync(email: string, password: string) {
    return this.apiService.post("accounts/login", { email, password });
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
    if (!this.firstMatchesLoad)
      return

    this.$user.subscribe({
      next: (user) => {
        if (!user)
          return

        this.apiService.get(`matches`).subscribe({
          next: (response: any) => {
            this.accountMatchesSubject.next(response.data)
            this.firstMatchesLoad = false;
          },
          error: () => {
            this.toastService.showToast("Não foi possível obter as partidas", false);
          }
        })
      }
    })
  }
}
