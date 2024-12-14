import { Component } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { ApiService } from '../../services/api/api.service';

@Component({
  selector: 'app-login-google-btn',
  standalone: true,
  imports: [],
  templateUrl: './login-google-btn.component.html',
  styleUrl: './login-google-btn.component.css'
})
export class LoginGoogleBtnComponent {

  constructor (private apiService: ApiService) {}

  handleClick() {
    location.href = `${this.apiService.baseUrl}/oauth/google/login`
  }
}
