import { Component } from '@angular/core';
import { SmartquizDescComponent } from "../../components/smartquiz-desc/smartquiz-desc.component";
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { ToastService } from '../../services/toast/toast.service';
import { FormsModule } from '@angular/forms';
import { SpinnerLoadingComponent } from "../../components/spinner-loading/spinner-loading.component";
import { CommonModule } from '@angular/common';
import { ErrorUtils } from '../../utils/error-utils';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [SmartquizDescComponent, RouterLink, FormsModule, SpinnerLoadingComponent, CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  username = "";
  email = "";
  password = "";
  loading = false;

  constructor (private authService: AuthService, private toastService: ToastService, private route: Router) {}

  handleSubmit() {
    this.loading = true
    this.authService.registerAsync(this.username, this.email, this.password).subscribe({
      next: (response: any) => {
        this.loading = false;
        const token = response.data.token
        this.authService.setToken(token);
        location.href = "/";
      },
      error: (error) => {
        this.loading = false;
        this.toastService.showToast(ErrorUtils.getErrorFromResponse(error));
      }
    })
  }
}
