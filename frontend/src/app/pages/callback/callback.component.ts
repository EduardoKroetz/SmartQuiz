import { Component, OnInit } from '@angular/core';
import { SpinnerLoadingComponent } from "../../components/spinner-loading/spinner-loading.component";
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { ToastService } from '../../services/toast/toast.service';

@Component({
  selector: 'app-callback',
  standalone: true,
  imports: [SpinnerLoadingComponent],
  templateUrl: './callback.component.html',
  styleUrl: './callback.component.css'
})
export class CallbackComponent implements OnInit {

  constructor (private activatedRoute: ActivatedRoute, private router: Router, private authService: AuthService, private toastService: ToastService) {}

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      const token = params['token'];
      if (token)
      {
        this.authService.setToken(token);
        this.toastService.showToast("Login efetuado com sucesso!", true);
        this.router.navigate(['/']);
      }else {
        this.toastService.showToast("Token não encontrado. Faça login novamente.");
        this.router.navigate(['/login']);
      }
    })
  }
}
