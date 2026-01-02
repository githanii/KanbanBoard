import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Board } from '../core/models/board';



@Injectable({ providedIn: 'root' })
export class BoardService {
    board: Board[] = [];

  addList(id: number, newListTitle: string) {
    throw new Error('Method not implemented.');
  }
  deleteList(id: number, id1: any) {
    throw new Error('Method not implemented.');
  }
  addCard(id: number, id1: any, title: string) {
    throw new Error('Method not implemented.');
  }
  deleteCard(id: number, id1: any, id2: any) {
    throw new Error('Method not implemented.');
  }
  moveCard(id: number, id1: any, id2: any, id3: any, currentIndex: number) {
    throw new Error('Method not implemented.');
  }
  private readonly apiUrl = 'https://localhost:7178/api/Board';

  constructor(private http: HttpClient) {}

  getBoard(): Observable<Board[]> {
    return this.http.get<Board[]>(this.apiUrl);
  }

  getBoardById(id: number): Observable<Board> {
    return this.http.get<Board>(`${this.apiUrl}/${id}`);
  }

  createBoard(name: string): Observable<Board> {
    return this.http.post<Board>(this.apiUrl, { name });
  }

  deleteBoard(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
