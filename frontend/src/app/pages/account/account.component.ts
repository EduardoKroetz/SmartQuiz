import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../services/account/account.service';
import Account from '../../interfaces/Account';
import { BackIconComponent } from "../../components/back-icon/back-icon.component";

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [BackIconComponent],
  templateUrl: './account.component.html',
  styleUrl: './account.component.css'
})
export class AccountComponent implements OnInit {
  account: Account | null = null;

  constructor (private accountService: AccountService) {}

  ngOnInit(): void {
    this.accountService.$user.subscribe({
      next: (data) => {
        this.account = data
      }
    })
  }
}
