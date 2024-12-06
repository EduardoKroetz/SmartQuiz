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
}
