import { Component, Input } from '@angular/core';
import { Match } from '../../interfaces/Match';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { DateUtils } from '../../utils/date-utils';

@Component({
  selector: 'app-history-item',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './history-item.component.html',
  styleUrl: './history-item.component.css'
})
export class HistoryItemComponent {
  @Input() match: Match | null = null;

  formatDate(date: Date) {
    return DateUtils.FormatDate(date);
  }
}
