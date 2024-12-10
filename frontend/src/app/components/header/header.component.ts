import { Component } from '@angular/core';
import { SearchBarComponent } from "../search-bar/search-bar.component";
import { SidebarService } from '../../services/sidebar/sidebar.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [SearchBarComponent],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  sidebarIsOpen = false;

  constructor (private sidebarService: SidebarService) {
    sidebarService.$isOpen.subscribe({
      next: (isOpen) => {
        this.sidebarIsOpen = isOpen;
      }
    })
  }

  toggleSidebar() {
    this.sidebarService.toggleSidebar();
  }
}
