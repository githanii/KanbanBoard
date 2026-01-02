import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BoardDetails } from '../core/models/board';

@Injectable({ providedIn: 'root' })
export class BoardService {
  private readonly apiUrl = 'https://localhost:5001/api/boards';

  constructor(private http: HttpClient) {}

  getBoardDetails(id: number): Observable<BoardDetails> {
    return this.http.get<BoardDetails>(`${this.apiUrl}/${id}`);
  }
}
