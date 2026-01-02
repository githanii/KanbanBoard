import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { CardModule } from 'primeng/card';
import { ToastModule } from 'primeng/toast';
import { DialogModule } from 'primeng/dialog';
import { TooltipModule } from 'primeng/tooltip';
import { MessageService } from 'primeng/api';

import { ListService } from '../../services/list.service';
import { CardService } from '../../services/card.service';
import { BoardService } from '../../services/board.service';

import { List, Board, Card } from '../../core/models/board';

@Component({
  selector: 'app-board-details',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ButtonModule,
    InputTextModule,
    CardModule,
    ToastModule,
    DialogModule,
    TooltipModule
  ],
  providers: [MessageService],
  templateUrl: './board-details.component.html',
  styleUrls: ['./board-details.component.css']
})
export class BoardDetailsComponent implements OnInit {

  boardId = 0;
  boardName = '';

  lists: List[] = [];

  loading = true;
  errorMsg = '';

  // For adding new cards
  newCardTitles: { [listId: number]: string } = {};
  
  // For adding new list
  showAddList = false;
  newListName = '';

  // For add card dialog
  showAddCardDialog = false;
  selectedListForCard: number | null = null;
  newCardTitle = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private listService: ListService,
    private cardService: CardService,
    private boardService: BoardService,
    private toast: MessageService
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    this.boardId = Number(idParam);

    if (!this.boardId || isNaN(this.boardId)) {
      this.loading = false;
      this.errorMsg = 'Invalid board ID.';
      this.toast.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Invalid board ID'
      });
      return;
    }

    this.loadBoardName();
    this.loadListsAndCards();
  }

  private loadBoardName(): void {
    this.boardService.getBoardById(this.boardId).subscribe({
      next: (b: Board) => {
        this.boardName = b?.name || `Board #${this.boardId}`;
      },
      error: (err) => {
        console.error('Error loading board name:', err);
        this.boardName = `Board #${this.boardId}`;
      }
    });
  }

  private loadListsAndCards(): void {
    this.loading = true;
    this.errorMsg = '';

    this.listService.getLists().subscribe({
      next: (allLists: List[]) => {
        const filtered = (allLists || []).filter(l => {
          const listBoardId = (l as any).boardId || (l as any).BoardId;
          return listBoardId === this.boardId;
        });

        filtered.sort((a, b) => (a.order ?? 0) - (b.order ?? 0));

        filtered.forEach(list => {
          this.loadCardsForList(list);
        });

        this.lists = filtered;
        this.loading = false;

        if (this.lists.length === 0) {
          this.toast.add({
            severity: 'info',
            summary: 'No Lists',
            detail: 'This board has no lists yet. Click "Add List" to create one.'
          });
        }
      },
      error: (err) => {
        console.error('Error loading lists:', err);
        this.loading = false;
        this.errorMsg = 'Failed to load lists.';
        this.toast.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to load lists'
        });
      }
    });
  }

  private loadCardsForList(list: List): void {
    this.cardService.getCardsByListId(list.id).subscribe({
      next: (cards: Card[]) => {
        list.cards = cards || [];
        list.cards.sort((a, b) => (a.position ?? 0) - (b.position ?? 0));
      },
      error: (err: any) => {
        console.error(`Error loading cards for list ${list.id}:`, err);
        list.cards = [];
      }
    });
  }

  openAddListDialog(): void {
    this.newListName = '';
    this.showAddList = true;
  }

  createList(): void {
    const name = (this.newListName || '').trim();
    if (!name) {
      this.toast.add({
        severity: 'warn',
        summary: 'Required',
        detail: 'List name is required'
      });
      return;
    }

    this.listService.addList(this.boardId, name).subscribe({
      next: (newList) => {
        this.showAddList = false;
        this.toast.add({
          severity: 'success',
          summary: 'Success',
          detail: 'List created successfully'
        });
        
        newList.cards = [];
        this.lists.push(newList);
      },
      error: (err) => {
        console.error('Error creating list:', err);
        this.toast.add({
          severity: 'error',
          summary: 'Error',
          detail: err?.error?.message || err?.message || 'Failed to create list'
        });
      }
    });
  }

  // Delete a list - FIXED
  deleteList(list: List): void {
    const listName = list.title || (list as any).title || 'this list';
    
    if (!confirm(`Are you sure you want to delete "${listName}"?`)) {
      return;
    }

    console.log('Attempting to delete list:', list.id);

    this.listService.deleteList(list.id).subscribe({
      next: () => {
        console.log('List deleted successfully');
        this.lists = this.lists.filter(l => l.id !== list.id);
        this.toast.add({
          severity: 'success',
          summary: 'Success',
          detail: 'List deleted successfully'
        });
      },
      error: (err: any) => {
        console.error('Error deleting list:', err);
        this.toast.add({
          severity: 'error',
          summary: 'Error',
          detail: err?.error?.message || 'Failed to delete list'
        });
      }
    });
  }

  deleteCard(cardId: number, listId: number): void {
    if (!confirm('Are you sure you want to delete this card?')) {
      return;
    }

    console.log('Attempting to delete card:', cardId, 'from list:', listId);

    this.cardService.deleteCard(cardId).subscribe({
      next: () => {
        console.log('Card deleted successfully');
        const list = this.lists.find(l => l.id === listId);
        if (list && list.cards) {
          list.cards = list.cards.filter(c => c.id !== cardId);
          this.toast.add({
            severity: 'success',
            summary: 'Success',
            detail: 'Card deleted successfully'
          });
        }
      },
      error: (err: any) => {
        console.error('Error deleting card:', err);
        this.toast.add({
          severity: 'error',
          summary: 'Error',
          detail: err?.error?.message || 'Failed to delete card'
        });
      }
    });
  }

  addCardToList(listId: number, title: string): void {
    const t = title.trim();
    if (!t) {
      this.toast.add({
        severity: 'warn',
        summary: 'Required',
        detail: 'Card title is required'
      });
      return;
    }

    this.cardService.addCard(listId, t).subscribe({
      next: (newCard: Card) => {
        const list = this.lists.find(l => l.id === listId);
        if (list) {
          if (!list.cards) list.cards = [];
          list.cards.push(newCard);
          list.cards.sort((a, b) => (a.position ?? 0) - (b.position ?? 0));
          this.newCardTitles[listId] = '';
          this.toast.add({
            severity: 'success',
            summary: 'Success',
            detail: 'Card added successfully'
          });
        }
      },
      error: (err: any) => {
        console.error('Error adding card:', err);
        this.toast.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to add card'
        });
      }
    });
  }

  addCardFromDialog(): void {
    if (!this.selectedListForCard || !this.newCardTitle.trim()) {
      this.toast.add({
        severity: 'warn',
        summary: 'Required',
        detail: 'Please select a list and enter a card title'
      });
      return;
    }

    this.addCardToList(this.selectedListForCard, this.newCardTitle);
    this.showAddCardDialog = false;
    this.selectedListForCard = null;
    this.newCardTitle = '';
  }

  goBack(): void {
    this.router.navigate(['/board']);
  }

  refreshBoard(): void {
    this.loadListsAndCards();
  }
}