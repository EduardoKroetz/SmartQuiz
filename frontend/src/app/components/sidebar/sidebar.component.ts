import { Component, OnDestroy, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { SidebarService } from '../../services/sidebar/sidebar.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent implements OnInit {
  currentSection: string = 'home';
  isOpen = false;

  constructor (private sidebarService: SidebarService) {
    sidebarService.$isOpen.subscribe({
      next: (isOpen) => {
        this.isOpen = isOpen
      }
    })
  }
  
  ngOnInit(): void {
    document.addEventListener('click', (ev) => this.handleCloseSidebar(ev));
  }

  setSection(section: string): void {
    this.currentSection = section;
    this.toggleSidebar()
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
