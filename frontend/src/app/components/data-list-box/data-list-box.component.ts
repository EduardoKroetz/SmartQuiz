import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-data-list-box',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './data-list-box.component.html',
  styleUrl: './data-list-box.component.css'
})
export class DataListBoxComponent {
  @Input() title = "";
  @Input() redirect = "";
}
