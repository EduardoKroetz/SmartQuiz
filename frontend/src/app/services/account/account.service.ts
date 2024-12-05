import { Injectable } from '@angular/core';
import { ApiService } from '../api/api.service';
import { BehaviorSubject } from 'rxjs';
import User from '../../interfaces/User';
import { ToastService } from '../toast/toast.service';
import { Quiz } from '../../interfaces/Quiz';

@Injectable({
  providedIn: 'root'
})
export class AccountService {  
  private userSubject = new BehaviorSubject<User | null>(null);
  private userQuizzesSubject = new BehaviorSubject<Quiz[]>([]);

  $user = this.userSubject.asObservable();
  $quizzes = this.userQuizzesSubject.asObservable();

  constructor (private apiService: ApiService, private toastService: ToastService) {
    this.setUser();
    this.getAccountQuizzes();
  }

  setUser() {
    this.apiService.get("accounts").subscribe({
      next: (response: any) => {
        this.userSubject.next(response.data);
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

  private getAccountQuizzes() {
    this.$user.subscribe({
      next: (user) => {
        if (!user)
          return
        this.apiService.get(`accounts/${user.id}/quizzes`).subscribe({
          next: (response: any) => {
            this.userQuizzesSubject.next(response.data)
          },
          error: () => {
            this.toastService.showToast("Não foi possível obter os quizzes", false);
          }
        })
      }
    })
  }
}
