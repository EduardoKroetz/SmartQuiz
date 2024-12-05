import { Component } from '@angular/core';
import { SmartquizDescComponent } from "../../components/smartquiz-desc/smartquiz-desc.component";
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [SmartquizDescComponent, RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {

}
