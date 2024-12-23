import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { SidebarService } from '../../services/sidebar/sidebar.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent implements OnInit {
  currentRoute: string = '';
  isOpen = false;

  constructor (private sidebarService: SidebarService, private router: Router) {}
  
  ngOnInit(): void {
    this.sidebarService.$isOpen.subscribe({
      next: (isOpen) => {
        this.isOpen = isOpen
      }
    })

    this.currentRoute = this.router.url;

    this.router.events.subscribe(() => {
      this.currentRoute = this.router.url;;
    })

    document.addEventListener('click', (ev) => this.handleCloseSidebar(ev));
  }

  toggleSidebar() {
    this.sidebarService.toggleSidebar();
  }

  handleCloseSidebar(ev: Event) {
    if (!this.isOpen)
      return
    
    const sidebarElement = document.querySelector('.sidebar');
    const burgerElement = document.querySelector('.burger');
    const target = ev.target as HTMLElement;

    if (sidebarElement && !sidebarElement.contains(target) && burgerElement && !burgerElement?.contains(target))
    {
      this.sidebarService.close();
      document.removeEventListener('click', (ev) => this.handleCloseSidebar(ev));
    }
  }

}
