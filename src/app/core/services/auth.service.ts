import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'

})
export class AuthService {
  private readonly TOKEN_KEY = 'jwt';
private apiUrl = 'https://localhost:7178/api/auth';

  constructor(private http: HttpClient, private router: Router) { }
  login(username: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, { username, password }).pipe(
      tap(response => {
        this.setToken(response.accessToken);
        this.router.navigate(['/']);


      }
      ));
  }
  register(username: string, password: string, Email: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/register`, { username, password, Email }).pipe(
      tap(response => {
        console.log('User registered:', response);
        this.router.navigate(['/']);
      })
    );
  }


  setToken(token: string): void {
    localStorage.setItem(this.TOKEN_KEY, token);
  }
  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  logout() {
    localStorage.removeItem(this.TOKEN_KEY);
    this.router.navigate(['/login']);

  }
  isLoggedIn(): boolean {
    return !!this.getToken();
  }
  getAuthHeader(): { Authorization: string } | {} {
    const token = this.getToken();
    return token ? { Authorization: `Bearer ${token}` } : {}; 
}

  isAdmin(): boolean {
    const token = this.getToken();
    if (!token) return false;
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.role === 'Admin';
    } catch (e) {
      console.error('Error decoding token:', e);
      return false;
    }
  } }