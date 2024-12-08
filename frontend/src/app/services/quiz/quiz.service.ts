import { Injectable } from '@angular/core';
import { ApiService } from '../api/api.service';
import { CreateQuiz } from '../../interfaces/Quiz';
import { CreateQuestion } from '../../interfaces/Question';

@Injectable({
  providedIn: 'root'
})
export class QuizService {

  constructor(private apiService: ApiService) { }

  getQuizById(id: string) {
    return this.apiService.get(`quizzes/${id}`);
  }

  getQuizQuestions(id: string) {
    return this.apiService.get(`quizzes/${id}/questions`);
  }

  createQuiz(props: CreateQuiz) {
    return this.apiService.post('quizzes', props);
  }

  createQuizQuestion(question: CreateQuestion) {
    return this.apiService.post('questions', question);
  }

  toggleQuiz(quizId: string) {
    return this.apiService.post('quizzes/toggle/'+quizId, {})
  }

  deleteQuiz(quizId: string) {
    return this.apiService.delete(`quizzes/${quizId}`);
  }
}
