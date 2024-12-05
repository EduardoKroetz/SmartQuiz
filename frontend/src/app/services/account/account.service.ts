import { Injectable } from '@angular/core';
import { ApiService } from '../api/api.service';
import { BehaviorSubject } from 'rxjs';
import User from '../../interfaces/User';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  token : string | null = null;
  keyToken = "auth-token";
  private userSubject = new BehaviorSubject<User | null>(null);
  
  $user = this.userSubject.asObservable();

  constructor (private apiService: ApiService) {}

  setUser() {
    this.apiService.get("accounts").subscribe({
      next: (response: any) => {
        console.log(response);
        this.userSubject.next(response);
      }
    });
  }

  setToken(token: string) {
    localStorage.setItem(this.keyToken, token)
  }

  loginAsync(email: string, password: string) {
    return this.apiService.post("accounts/login", { email, password });
  }
}
