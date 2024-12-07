import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { QuizService } from '../../services/quiz/quiz.service';
import { ToastService } from '../../services/toast/toast.service';
import { CreateQuestion, createQuestionDefault } from '../../interfaces/Question';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-create-quiz-questions',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './create-quiz-questions.component.html',
  styleUrl: './create-quiz-questions.component.css'
})
export class CreateQuizQuestionsComponent implements OnInit, OnDestroy {
  createQuestion: CreateQuestion = { ...createQuestionDefault };
  selectedCorrectOption: number = -1; 
  private sub: any;

  constructor (private activatedRoute: ActivatedRoute, private quizService: QuizService, private toastService: ToastService, private route:Router) {}

  ngOnInit(): void {
    const quizId = this.activatedRoute.snapshot.paramMap.get('id') ?? '';
    this.sub = this.activatedRoute.paramMap.subscribe(params => {
      this.createQuestion = JSON.parse(JSON.stringify(createQuestionDefault));
      this.createQuestion.order = parseInt(params.get('order') ?? '-1');
      this.createQuestion.quizId = quizId;
      this.selectedCorrectOption = -1;
    })
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  createNewQuestion() {
    if (this.selectedCorrectOption === -1)
    {
      this.toastService.showToast("Informe a opção correta");
      return
    }

    this.createQuestion.options[this.selectedCorrectOption].isCorrectOption = true;
    this.quizService.createQuizQuestion(this.createQuestion).subscribe({
      next: () => {
        this.toastService.showToast("Questão criada com sucesso!", true);
        this.route.navigate([`/quizzes/create-quiz/${this.createQuestion.quizId}/questions/${this.createQuestion.order + 1}`])
      },
      error: (error: any) => {
        this.createQuestion.options[this.selectedCorrectOption].isCorrectOption = false;
        this.toastService.showToast(error.error.errors[0]);
      }
    });
  }

  toggleQuiz() {
    this.quizService.toggleQuiz(this.createQuestion.quizId).subscribe({
      next: () => {
        this.toastService.showToast("Quiz ativado com sucesso!", true);
        this.route.navigate([`/quizzes/${this.createQuestion.quizId}`])
      },
      error: (error: any) => {
        this.toastService.showToast(error.error.errors[0]);
      }
    });
  }  
}
