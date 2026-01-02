import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Card } from '../core/models/board';

@Injectable({
  providedIn: 'root'
})
export class CardService {
  private apiUrl = 'https://localhost:7178/api/Cards';
  
  constructor(private http: HttpClient) { }

  getCards(): Observable<Card[]> {
    return this.http.get<Card[]>(this.apiUrl);
  }

  getCardsByListId(listId: number): Observable<Card[]> {
    return this.http.get<Card[]>(`${this.apiUrl}/list/${listId}`);
  }

  addCard(listId: number, title: string, description: string = ''): Observable<Card> {
    return this.http.post<Card>(`${this.apiUrl}/${listId}`, { 
      title: title,
      description: description
    });
  }

  deleteCard(cardId: number) {
  return this.http.delete(
    `https://localhost:7178/api/Cards/${cardId}`
  );
}


  updateCard(cardId: number, title: string, description: string): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${cardId}/update`, { 
      title: title,
      description: description
    });
  }

  moveCard(cardId: number, targetListId: number, targetOrderIndex: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${cardId}/move`, {
      targetListId: targetListId,
      targetOrderIndex: targetOrderIndex
    });
  }
}