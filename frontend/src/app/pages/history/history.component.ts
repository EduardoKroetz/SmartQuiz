import { Component } from '@angular/core';
import { HistoryItemComponent } from "../../components/history-item/history-item.component";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-history',
  standalone: true,
  imports: [HistoryItemComponent, CommonModule],
  templateUrl: './history.component.html',
  styleUrl: './history.component.css'
})
export class HistoryComponent {

}
