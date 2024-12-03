import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-presentation-box',
  standalone: true,
  imports: [],
  templateUrl: './presentation-box.component.html',
  styleUrl: './presentation-box.component.css'
})
export class PresentationBoxComponent {
  @Input()  title: string = ""; 
  @Input()  data: any; 

}
