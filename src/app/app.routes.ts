import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { BoardDetailsComponent } from './board/board-details/board-details.component';


export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },

  {
    path: 'login',
    loadComponent: () =>
      import('./auth/login/login.component').then(m => m.LoginComponent)
  },
  { path: 'register',
    loadComponent: () =>
      import('./auth/register/register.component').then(m => m.RegisterComponent)
  },
    {   path: 'board',
    loadComponent: () =>
      import('./board/board/board.component').then(m => m.BoardComponent)
  },
  
 {   path: 'boarddetail/:id',
    loadComponent: () =>
      import('./board/board-details/board-details.component').then(m => m.BoardDetailsComponent)
  },
  
];