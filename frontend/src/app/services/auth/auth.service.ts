import { Injectable } from '@angular/core';
import { ApiService } from '../api/api.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private apiService: ApiService) { }

  setToken(token: string) {
    localStorage.setItem("auth-token", token)
    this.apiService.setAuthorization();
  }

  loginAsync(email: string, password: string) {
    return this.apiService.post("accounts/login", { email, password });
  }

  registerAsync(username: string, email: string, password: string) {
    return this.apiService.post("accounts/register", { username, email, password });
  }

  logout() {
    localStorage.removeItem("auth-token");
    location.href = "/login";
  }
}
