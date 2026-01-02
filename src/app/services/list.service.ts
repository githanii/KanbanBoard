import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { List } from '../core/models/board';

@Injectable({
  providedIn: 'root'
})
export class ListService {
  private apiUrl = 'https://localhost:7178/api/Lists';
  
  constructor(private http: HttpClient) { }

  getLists(): Observable<List[]> {
    return this.http.get<List[]>(this.apiUrl);
  }

  addList(boardId: number, title: string): Observable<List> {
    return this.http.post<List>(`${this.apiUrl}/${boardId}`, { title });
  }

  deleteList(id: number): Observable<void> {
     console.log('Deleting list:', id);
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  updateList(id: number, title: string, orderIndex?: number): Observable<List> {
    return this.http.put<List>(`${this.apiUrl}/${id}`, { title, orderIndex });
  }

  getListById(id: number): Observable<List> {
    return this.http.get<List>(`${this.apiUrl}/${id}`);
  }
}