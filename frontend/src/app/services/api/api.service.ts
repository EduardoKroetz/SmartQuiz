import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService implements OnInit {
  private baseUrl = 'https://localhost:7077/api'
  private httpHeaders = new HttpHeaders();

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    const storedToken = localStorage.getItem("auth-token");
    const token = JSON.stringify(storedToken);
    this.httpHeaders = this.httpHeaders.set('Authorization', token);
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
