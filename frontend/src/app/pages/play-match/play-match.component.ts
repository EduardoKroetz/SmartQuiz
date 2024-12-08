import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../services/account/account.service';
import { MatchService } from '../../services/match/match.service';
import { Question } from '../../interfaces/Question';
import { Match } from '../../interfaces/Match';
import Account from '../../interfaces/Account';
import { ToastService } from '../../services/toast/toast.service';
import { CommonModule, Location } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-play-match',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './play-match.component.html',
  styleUrl: './play-match.component.css'
})
export class PlayMatchComponent implements OnInit {
  matchId = '';
  currentQuestion : Question | null = null;
  match: Match | null = null;
  account: Account | null = null;
  optionId: string | null = null;
  isLastQuestion = false;
  finished = false;
  expires = true;

  constructor (private activatedRoute: ActivatedRoute, private accountService: AccountService, private matchService: MatchService, private toastService: ToastService, private location: Location, private route: Router) {}

  ngOnInit(): void {
    this.matchId = this.activatedRoute.snapshot.paramMap.get('id') || '';
    this.setMatch();
    this.setCurrentQuestion()
  }

  setAccount() {
    this.accountService.$user.subscribe({
      next: (data) => {
        this.account = data;
      }
    })
  }

  setMatch() {
    this.matchService.getMatchById(this.matchId).subscribe({
      next: (response: any) => {
        this.match = response.data;
        this.expires = this.match!.quiz!.expires;
        if (this.expires)
          {
            const intervalId = setInterval(() => {
              if (this.match)
              {
                if (this.match.remainingTimeInSeconds > 0)
                {
                  this.match.remainingTimeInSeconds--
                }
                else if (this.finished) {
                  clearInterval(intervalId);
                  
                } else {
                  clearInterval(intervalId);
                  this.failMatch();
                }
              }
            },1000);
          }
      },
      error: () => {
        this.toastService.showToast("Não foi possível obter os dados da partida", false);
        setTimeout(() => { this.location.back() }, 500)
      }
    })
  }

  setCurrentQuestion() {
    this.matchService.getNextQuestion(this.matchId).subscribe({
      next: (response: any) => {
        this.isLastQuestion = response.data.isLastQuestion;
        this.currentQuestion = response.data.question;
      },
      error: (error) => {
        this.toastService.showToast(error.error.errors[0]);
        setTimeout(() => { this.location.back() }, 500)
      }
    })
  }

  submitResponse() {
    if (!this.optionId)
    {
      this.toastService.showToast("Informe uma opção", false)
      return
    }
    this.matchService.submitResponse(this.matchId, this.optionId).subscribe({
      next: () => {
        this.optionId = null;
        if (this.isLastQuestion) {
          this.toastService.showToast("Partida finalizada com sucesso!", true)
          this.finished = true;
          this.route.navigate(['matches/'+ this.matchId])
        }else {
          this.setCurrentQuestion();
        }
      },
      error: (error) => {
        this.toastService.showToast(error.error.errors[0]);
      }
    });
  }

  failMatch() {
    this.matchService.failMatch(this.matchId).subscribe({
      next: () => {
        this.toastService.showToast("Tempo expirado");
        this.route.navigate(['matches/'+ this.matchId])
      },
      error: () => {
        this.toastService.showToast("Não foi possível atualizar o status da partida")
      }
    })
  }
}
