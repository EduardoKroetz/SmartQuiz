import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-password-form',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './password-form.component.html',
  styleUrl: './password-form.component.css'
})
export class PasswordFormComponent {
  @Output() passwordChange = new EventEmitter<string>();
  @Input() passwordError : string | null = null;
  @Input() isLogin = false;

  isVisible = false;

  onPasswordChange(event: Event) {
    const inputElement = event.target as HTMLInputElement
    this.passwordChange.emit(inputElement.value); 
  }

  toggleVisibility() {
    this.isVisible = !this.isVisible;
  }
}
