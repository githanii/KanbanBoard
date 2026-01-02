import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CardModule } from 'primeng/card';
import { PasswordModule } from 'primeng/password';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';

import { AuthService } from '../../core/services/auth.service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [
    CardModule,
    InputTextModule,
    PasswordModule,
    ButtonModule,
    FormsModule,
    RouterModule,

  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  Email: string = '';
  username: string = '';
  password: string = '';


  constructor(private authService: AuthService) { }
  ngOnInit(): void {
  }

  register(): void {
    
    this.authService.register(this.username, this.password, this.Email).subscribe({
      next: () => {
        alert('Registration successful! You can now log in.');

      },
      error: err => {
        alert('Registration failed: ' + (err.error?.message || 'Password or Username is not valid'));
      }
    });
  }
}