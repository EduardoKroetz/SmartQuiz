import { Component, OnInit } from '@angular/core';
import { ToastService } from '../../services/toast/toast.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-toast',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './toast.component.html',
  styleUrl: './toast.component.css'
})
export class ToastComponent implements OnInit {
  message = "";
  color: string = '#4CAF50';
  isOpen = false;

  constructor (private toastService: ToastService) {}

  ngOnInit(): void {
    this.toastService.toast$.subscribe(toast => {
      this.message = toast.message;
      this.color = toast.color;
      this.isOpen = toast.isOpen;
    })
  }

  close() {
    this.toastService.closeToast();
  }
}
