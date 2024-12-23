import { Component } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { ApiService } from '../../services/api/api.service';
import { SpinnerLoadingComponent } from "../spinner-loading/spinner-loading.component";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login-google-btn',
  standalone: true,
  imports: [SpinnerLoadingComponent, CommonModule],
  templateUrl: './login-google-btn.component.html',
  styleUrl: './login-google-btn.component.css'
})
export class LoginGoogleBtnComponent {
  isLoading = false;
  
  constructor (private apiService: ApiService) {}

  handleClick() {
    this.isLoading = true;
    location.href = `${this.apiService.baseUrl}/oauth/google/login`
  }
}
