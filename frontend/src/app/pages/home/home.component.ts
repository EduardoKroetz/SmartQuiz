import { Component } from '@angular/core';
import { HeaderComponent } from "../../components/header/header.component";
import { SidebarComponent } from "../../components/sidebar/sidebar.component";
import { PresentationBoxComponent } from "../../components/presentation-box/presentation-box.component";
import { DataListBoxComponent } from "../../components/data-list-box/data-list-box.component";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [HeaderComponent, SidebarComponent, PresentationBoxComponent, DataListBoxComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  maxScore = 0;
  minTime = 0;
  matchesPlayed = 0;
  totalScore = 0;
  correctAnswers = 0;
  createdQuizzes = 0;
}
