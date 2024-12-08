import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConfirmationToastService {
  private toastSubject = new Subject<{message: string, isOpen: boolean }>();
  toast$ = this.toastSubject.asObservable();
  private confirmedSubject = new Subject<null | boolean>();
  confirmed$ = this.confirmedSubject.asObservable();

  showToast(message: string) {
    this.toastSubject.next({ message, isOpen: true });
  }

  closeToast() {
    this.toastSubject.next({ message: "", isOpen: false });
    this.confirmedSubject.next(false);
  }

  confirm() {
    this.confirmedSubject.next(true);
    this.toastSubject.next({ message: "", isOpen: false });
  }
}
