import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = 'https://localhost:7077/api'
  private httpHeaders = new HttpHeaders();

  constructor(private http: HttpClient) { 
    const token = localStorage.getItem("auth-token");
    this.httpHeaders = this.httpHeaders.set('Authorization', 'Bearer '+ token);
  }

  get<T>(endpoint: string, params?: any): Observable<T> {
    const httpParams = new HttpParams({ fromObject: params || {} });
    return this.http.get<T>(`${this.baseUrl}/${endpoint}`, { headers: this.httpHeaders, params: httpParams });
  }

  post<T>(endpoint: string, body: any): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}/${endpoint}`, body, { headers: this.httpHeaders});
  }

  put<T>(endpoint: string, body: any): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}/${endpoint}`, body, { headers: this.httpHeaders });
  }

  delete<T>(endpoint: string): Observable<T> {
    return this.http.delete<T>(`${this.baseUrl}/${endpoint}`, { headers: this.httpHeaders});
  }
}
