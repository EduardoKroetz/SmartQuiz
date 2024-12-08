import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BackIconComponent } from "../../components/back-icon/back-icon.component";

@Component({
  selector: 'app-generate-quiz',
  standalone: true,
  imports: [CommonModule, FormsModule, BackIconComponent],
  templateUrl: './generate-quiz.component.html',
  styleUrl: './generate-quiz.component.css'
})
export class GenerateQuizComponent {
  theme = "";
  difficulty = "";
  numberOfQuestions = 0;
  expires = true;
  expiresInSeconds = 0;
}
