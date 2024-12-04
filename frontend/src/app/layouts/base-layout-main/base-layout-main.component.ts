import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-base-layout-main',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './base-layout-main.component.html',
  styleUrl: './base-layout-main.component.css',
})
export class BaseLayoutMainComponent {

}
