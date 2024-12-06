import { Injectable } from '@angular/core';
import { ApiService } from '../api/api.service';

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
}
