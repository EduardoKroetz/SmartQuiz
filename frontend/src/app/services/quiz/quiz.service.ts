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

  generateQuiz(theme: string, difficulty: string, numberOfQuestions: number, expires: boolean, expiresInSeconds: number) {
    return this.apiService.post('quizzes/generate', { theme, difficulty, numberOfQuestions, expires, expiresInSeconds })
  }

  searchQuizzes(reference: string, pageSize: number, pageNumber: number) {
    return this.apiService.get(`quizzes/search?reference=${reference}&pageSize=${pageSize}&pageNumber=${pageNumber}`)
  }
}
