import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { QuizService } from '../../services/quiz/quiz.service';
import { ToastService } from '../../services/toast/toast.service';
import { Quiz } from '../../interfaces/Quiz';
import { CommonModule } from '@angular/common';
import { QuizCardComponent } from "../../components/quiz-card/quiz-card.component";
import { SpinnerLoadingComponent } from "../../components/spinner-loading/spinner-loading.component";

@Component({
  selector: 'app-search-results',
  standalone: true,
  imports: [CommonModule, QuizCardComponent, SpinnerLoadingComponent],
  templateUrl: './search-results.component.html',
  styleUrl: './search-results.component.css'
})
export class SearchResultsComponent implements OnInit {
  reference = "";
  pageSize = 15;
  pageNumber = 1;
  quizzes: Quiz[] = [];
  isMaximumResults = true;
  isLoadingResults = true;

  constructor (private activedRoute: ActivatedRoute, private quizService: QuizService, private toastService: ToastService) {}

  ngOnInit(): void {
    this.activedRoute.queryParams.subscribe(params => {
      this.reference = params['reference'];
      this.reset();
      this.search();
    })
  }

  search() {
    this.isLoadingResults = true;
    this.quizService.searchQuizzes(this.reference, this.pageSize, this.pageNumber).subscribe({
      next: (response: any) => {
        const quizzes : Quiz[] = response.data
        this.quizzes = [...this.quizzes, ...quizzes];
        if (quizzes.length < this.pageSize)
          this.isMaximumResults = true;
        else
          this.isMaximumResults = false
        this.isLoadingResults = false;
      },
      error: (error) => {
        this.isLoadingResults = false;
        this.toastService.showToast(error.error.errors[0])
      }
    })
  }

  loadMore() {
    this.pageNumber++;
    this.search();
  }

  reset() {
    this.quizzes = [];
    this.isMaximumResults = true;
    this.pageNumber = 1;
  }
}
