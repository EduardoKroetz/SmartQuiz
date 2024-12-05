import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  private toastSubject = new Subject<{message: string, color: string, isOpen: boolean }>();
  toast$ = this.toastSubject.asObservable();

  showToast(message: string, isSucess = false) {
    this.toastSubject.next({ message, color: isSucess ? '#4CAF50' : '#F44336', isOpen: true });
    setTimeout(() => {
      this.toastSubject.next({ message, color: isSucess ? '#4CAF50' : '#F44336', isOpen: false });
    }, 2.5 * 1000)
  }

  closeToast() {
    this.toastSubject.next({ message: "", color: 'gray', isOpen: false });
  }
}
