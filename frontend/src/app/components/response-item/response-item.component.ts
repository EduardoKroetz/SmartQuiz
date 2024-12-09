import { Component, Input } from '@angular/core';
import { Response } from '../../interfaces/Response';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-response-item',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './response-item.component.html',
  styleUrl: './response-item.component.css'
})
export class ResponseItemComponent {
  @Input() response : Response = null!;
  @Input() questionNumber = 0;

  detailsOpen = false;

  toggle() {
    this.detailsOpen = !this.detailsOpen;
  }
}
