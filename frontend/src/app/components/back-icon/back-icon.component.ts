import { Location } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-back-icon',
  standalone: true,
  imports: [],
  templateUrl: './back-icon.component.html',
  styleUrl: './back-icon.component.css'
})
export class BackIconComponent {

  constructor (private location: Location) {}

  back() {
    this.location.back();
  }
}
