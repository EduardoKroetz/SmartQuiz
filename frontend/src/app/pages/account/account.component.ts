import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../services/account/account.service';
import Account from '../../interfaces/Account';
import { BackIconComponent } from "../../components/back-icon/back-icon.component";
import { AuthService } from '../../services/auth/auth.service';
import { CommonModule } from '@angular/common';
import { ToastService } from '../../services/toast/toast.service';
import { FormsModule } from '@angular/forms';
import { SpinnerLoadingComponent } from "../../components/spinner-loading/spinner-loading.component";
import { ErrorUtils } from '../../utils/error-utils';

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [BackIconComponent, CommonModule, FormsModule, SpinnerLoadingComponent],
  templateUrl: './account.component.html',
  styleUrl: './account.component.css'
})
export class AccountComponent implements OnInit {
  account: Account | null = null;
  currentPassword = "";
  newPassword = "";
  isUpdatingInfo = false;
  isUpdatingPassword = false;

  constructor (public accountService: AccountService, private authService: AuthService, private toastService: ToastService) {}

  ngOnInit(): void {
    this.accountService.$user.subscribe({
      next: (data) => {
        this.account = data
      }
    })
  }

  logOut() {
    this.authService.logout();
    this.toastService.showToast("Você saiu da sua conta")
  }

  handleUpdate() {
    this.isUpdatingInfo = true;
    if (this.account == null)
      return

    this.accountService.update(this.account.username, this.account.email).subscribe({
      next: () => {
        this.isUpdatingInfo = false;
        this.toastService.showToast("Informações atualizadas com sucesso!", true);
      },
      error: (error) => {
        this.isUpdatingInfo = false;
        this.toastService.showToast(ErrorUtils.getErrorFromResponse(error));
      }
    });
  }

  handleUpdatePassword() {
    this.isUpdatingPassword = true;
    this.accountService.updatePassword(this.currentPassword, this.newPassword).subscribe({
      next: () => {
        this.isUpdatingPassword = false;
        this.newPassword = "";
        this.currentPassword = "";
        this.toastService.showToast("Senha atualizada com sucesso!", true)
      },
      error: (error) => {
        this.isUpdatingPassword = false;
        this.toastService.showToast(ErrorUtils.getErrorFromResponse(error), false)
      }
    })
  }
}
