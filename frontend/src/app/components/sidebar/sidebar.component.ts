import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AccountService } from '../../services/account/account.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  currentSection: string = 'home';

  setSection(section: string): void {
    this.currentSection = section;
  }

}
