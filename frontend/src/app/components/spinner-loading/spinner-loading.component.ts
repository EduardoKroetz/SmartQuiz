import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-spinner-loading',
  standalone: true,
  imports: [],
  templateUrl: './spinner-loading.component.html',
  styleUrl: './spinner-loading.component.css'
})
export class SpinnerLoadingComponent {
  @Input() isDark = true;
}
