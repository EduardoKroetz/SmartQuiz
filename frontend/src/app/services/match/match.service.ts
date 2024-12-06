import { Injectable } from '@angular/core';
import { ApiService } from '../api/api.service';

@Injectable({
  providedIn: 'root'
})
export class MatchService {

  constructor(private apiService: ApiService) { }

  getMatchById(id: string) {
    return this.apiService.get(`matches/${id}`);
  }

  getNextQuestion(matchId: string) {
    return this.apiService.get(`matches/${matchId}/next-question`);
  }

  submitResponse(matchId: string, questionOptionId: string) {
    return this.apiService.post(`matches/${matchId}/submit/${questionOptionId}`, {});
  }

  createMatch(quizId: string) {
    return this.apiService.post(`matches/play/quiz/${quizId}`, {});
  }
}
