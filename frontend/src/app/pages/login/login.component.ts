import { Component } from '@angular/core';
import { SmartquizDescComponent } from "../../components/smartquiz-desc/smartquiz-desc.component";
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ToastService } from '../../services/toast/toast.service';
import { CommonModule } from '@angular/common';
import { ErrorUtils } from '../../utils/error-utils';
import { AuthService } from '../../services/auth/auth.service';
import { SpinnerLoadingComponent } from "../../components/spinner-loading/spinner-loading.component";
import { PasswordFormComponent } from "../../components/password-form/password-form.component";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [SmartquizDescComponent, RouterLink, FormsModule, CommonModule, SpinnerLoadingComponent, PasswordFormComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  email = "";
  password = "";
  emailError: string | null = null;
  passwordError: string | null  = null;
  isLoading = false;

  constructor (private authService: AuthService, private toastService: ToastService) {}

  submit() {
    this.isLoading = true;
    this.emailError = null;
    this.passwordError = null;
    this.authService.loginAsync(this.email, this.password).subscribe({
      next: (response: any) => {
        this.authService.setToken(response.data.token);
        this.isLoading = false;
        location.href = "/";
      },
      error: (response: any) => {
        const errors: string[] = response.error.errors;
        this.emailError = ErrorUtils.getErrorMessage(errors, ['e-mail', 'E-mail', 'Email'])
        this.passwordError =  ErrorUtils.getErrorMessage(errors, ['senha'])
        if (!this.passwordError && !this.emailError)
        {
          this.toastService.showToast(errors[0], false);
        }
        this.isLoading = false;
      }
    });
  }

  handleChangePassword(password: string) {
    this.password = password;
  }

}
