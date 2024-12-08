import { Injectable } from '@angular/core';
import { ApiService } from '../api/api.service';
import { CreateQuestion } from '../../interfaces/Question';

@Injectable({
  providedIn: 'root'
})
export class QuestionService {
  
  constructor(private apiService: ApiService) { }

  createQuestion(question: CreateQuestion) {
    return this.apiService.post('questions', question);
  }

  deleteQuestion(questionId: string) {
    return this.apiService.delete('questions/'+ questionId);
  }
}
