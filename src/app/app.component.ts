import { Component } from '@angular/core';
import { Route, RouterModule, RouterOutlet, Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { BoardComponent } from './board/board/board.component';
import { BoardDetailsComponent } from './board/board-details/board-details.component';

const routes:Routes=[
  {path:'',redirectTo:'login', pathMatch: 'full'},
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'board', component: BoardComponent },
    {path: 'boardDetail/:id', component: BoardDetailsComponent}
];

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, RouterOutlet],
  template: '<router-outlet></router-outlet>',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'kANBANBOARD';
}
