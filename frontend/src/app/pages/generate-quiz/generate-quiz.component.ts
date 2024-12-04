import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-generate-quiz',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './generate-quiz.component.html',
  styleUrl: './generate-quiz.component.css'
})
export class GenerateQuizComponent {
  expires = true
}
