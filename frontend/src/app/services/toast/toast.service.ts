import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  private toastSubject = new Subject<{message: string, color: string, isOpen: boolean }>();
  toast$ = this.toastSubject.asObservable();

  showToast(message: string, color: string = '#4CAF50') {
    this.toastSubject.next({ message, color, isOpen: true });
  }

  closeToast() {
    this.toastSubject.next({ message: "", color: '#4CAF50', isOpen: false });
  }
}
