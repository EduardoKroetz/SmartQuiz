import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../services/account/account.service';
import User from '../../interfaces/User';

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [],
  templateUrl: './account.component.html',
  styleUrl: './account.component.css'
})
export class AccountComponent implements OnInit {
  account: User | null = null;

  constructor (private accountService: AccountService) {}

  ngOnInit(): void {
    this.accountService.$user.subscribe({
      next: (data) => 
        this.account = data
    })
  }
}
