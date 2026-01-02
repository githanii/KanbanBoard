import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService, MessageService } from 'primeng/api';
import {  BoardService } from '../../services/board.service';
import { AuthService } from '../../core/services/auth.service';
import { Board } from '../../core/models/board';
 


@Component({
  standalone: true,
  selector: 'app-board',
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    TableModule,
    ButtonModule,
    DialogModule,
    InputTextModule,
    ToastModule,
    ConfirmDialogModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.css']
})
export class BoardComponent implements OnInit {
  board: Board[] = [];
  loading = false;

    showCreate = false;
  newBoardName = '';

  constructor(
    private boardApi: BoardService,
    public auth: AuthService,
    private router: Router,
    private toast: MessageService,
    private confirm: ConfirmationService
  ) {}

  ngOnInit(): void {
    this.loadBoards();
  }

  loadBoards(): void {
    this.loading = true;
    this.boardApi.getBoard().subscribe({
      next: (res) => {
        this.board = res ?? [];
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        this.toast.add({
          severity: 'error',
          summary: 'Failed',
          detail: err?.error ?? 'Could not load board'
        });
      }
    });
  }

  openBoard(boardId: number): void {
  // Test navigation
  this.router.navigate(['/boarddetail', boardId]).then(
    success => console.log('Navigation success:', success),
    error => console.error('Navigation error:', error)
  );
}

  openCreateDialog(): void {
    this.newBoardName = '';
    this.showCreate = true;
  }

  createBoard(): void {
    const name = (this.newBoardName || '').trim();
    if (!name) {
      this.toast.add({ severity: 'warn', summary: 'Required', detail: 'Board name is required' });
      return;
    }

    this.boardApi.createBoard(name).subscribe({
      next: (created) => {
        this.showCreate = false;
        this.toast.add({ severity: 'success', summary: 'Created', detail: 'Board created successfully' });
        this.board = [created, ...this.board];
      },
      error: (err) => {
        this.toast.add({
          severity: 'error',
          summary: 'Failed',
          detail: err?.error ?? 'Could not create board'
        });
      }
    });
  }

  confirmDelete(board: Board): void {
    this.confirm.confirm({
      message: `Delete "${board.name}"?`,
      header: 'Confirm Delete',
      icon: 'pi pi-exclamation-triangle',
      accept: () => this.deleteBoard(board.id)
    });
  }

  deleteBoard(id: number): void {
    this.boardApi.deleteBoard(id).subscribe({
      next: () => {
        this.board = this.board.filter(b => b.id !== id);
        this.toast.add({ severity: 'success', summary: 'Deleted', detail: 'Board deleted' });
      },
      error: (err) => {
        this.toast.add({
          severity: 'error',
          summary: 'Failed',
          detail: err?.error ?? 'Could not delete board'
        });
      }
    });
  }
logout(): void {
  this.auth.logout();
  this.toast.add({
    severity: 'success',
    summary: 'Logged Out',
    detail: 'You have been logged out successfully'
  });
  this.router.navigate(['/login']); // Change '/login' to your login route
}

//  goAdminBoards(): void {
//      this.router.navigate(['/admin/boards']);
//   }

}