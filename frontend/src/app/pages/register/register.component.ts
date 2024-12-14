import { Component } from '@angular/core';
import { SmartquizDescComponent } from "../../components/smartquiz-desc/smartquiz-desc.component";
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { ToastService } from '../../services/toast/toast.service';
import { FormsModule } from '@angular/forms';
import { SpinnerLoadingComponent } from "../../components/spinner-loading/spinner-loading.component";
import { CommonModule } from '@angular/common';
import { ErrorUtils } from '../../utils/error-utils';
import { PasswordFormComponent } from "../../components/password-form/password-form.component";
import { LoginGoogleBtnComponent } from "../../components/login-google-btn/login-google-btn.component";
import { TermsComponent } from "../../components/terms/terms.component";

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [SmartquizDescComponent, RouterLink, FormsModule, SpinnerLoadingComponent, CommonModule, PasswordFormComponent, LoginGoogleBtnComponent, TermsComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  username = "";
  email = "";
  password = "";
  loading = false;

  usernameError : null | string = null;
  emailError : null | string = null;
  passwordError : null | string = null;

  constructor (private authService: AuthService, private toastService: ToastService, private route: Router) {}

  handleSubmit() {
    this.loading = true
    this.usernameError = null;
    this.emailError = null;
    this.passwordError = null;
    this.authService.registerAsync(this.username, this.email, this.password).subscribe({
      next: (response: any) => {
        this.loading = false;
        const token = response.data.token
        this.authService.setToken(token);
        location.href = "/";
      },
      error: (error) => {
        const errors: string[] = error.error.errors;
        this.emailError = ErrorUtils.getErrorMessage(errors, ['e-mail', 'E-mail', 'Email'])
        this.passwordError =  ErrorUtils.getErrorMessage(errors, ['senha'])
        this.usernameError =  ErrorUtils.getErrorMessage(errors, ['nome'])
        if (!this.passwordError && !this.emailError)
        {
          this.toastService.showToast(errors[0], false);
        }
        this.loading = false;
      }
    })
  }

  handleChangePassword(password: string) {
    this.password = password;
  }
}
