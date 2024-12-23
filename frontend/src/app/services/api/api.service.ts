import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  public baseUrl = environment.apiUrl;
  private httpHeaders = new HttpHeaders();

  constructor(private http: HttpClient) { 
    this.setAuthorization();
  }

  setAuthorization() {
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

  
  patch<T>(endpoint: string, body: any): Observable<T> {
    return this.http.patch<T>(`${this.baseUrl}/${endpoint}`, body, { headers: this.httpHeaders });
  }
}
