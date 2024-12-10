import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SidebarService {
  private isOpenSubject = new BehaviorSubject<boolean>(false);
  $isOpen = this.isOpenSubject.asObservable();

  constructor() { }

  toggleSidebar() {
    const currentValue = this.isOpenSubject.getValue();
    this.isOpenSubject.next(!currentValue) 
  }

  close() {
    this.isOpenSubject.next(false) 
  }
}
