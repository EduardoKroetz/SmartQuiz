import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ConfirmationToastService } from '../../services/confirmation-toast/confirmation-toast.service';

@Component({
  selector: 'app-confirmation-toast',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './confirmation-toast.component.html',
  styleUrl: './confirmation-toast.component.css'
})
export class ConfirmationToastComponent implements OnInit {
  message = "Deseja confirmar?";
  isOpen = false;

  constructor (private confirmationToastService: ConfirmationToastService) {
  }

  ngOnInit(): void {
    this.confirmationToastService.toast$.subscribe(toast => {
      this.message = toast.message;
      this.isOpen = toast.isOpen;
    })
  }

  close() {
    this.confirmationToastService.closeToast();
  }

  ok() {
    this.confirmationToastService.confirm();
  }
}
