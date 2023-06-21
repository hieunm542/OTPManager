import { Component } from '@angular/core';
import { AuthService } from './core';

@Component({
  selector: 'app-root',
  template: `
    <header>

      <nav class="navbar navbar-expand-lg navbar-light bg-light px-3" style="background-color: #e3f2fd;">
        <a class="navbar-brand" routerLink="">OTP Manager</a>
        <button
          class="navbar-toggler"
          type="button"
          data-toggle="collapse"
          data-target="#navbarSupportedContent"
          aria-controls="navbarSupportedContent"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span class="navbar-toggler-icon"></span>
        </button>

        <div class="collapse navbar-collapse" id="navbarSupportedContent">
          <ul class="navbar-nav me-auto">
            <li class="nav-item" routerLinkActive="active">
              <a class="nav-link" routerLink="tokens">Tokens</a>
            </li>
            <li class="nav-item" routerLinkActive="active">
              <a class="nav-link" routerLink="phones">Phones</a>
            </li>
            <li class="nav-item" routerLinkActive="active">
              <a class="nav-link" routerLink="sms">SMS</a>
            </li>
          </ul>
          <div class="form-inline">
            <button
              class="btn btn-outline-success my-2 my-sm-0"
              type="button"
              *ngIf="authService.user$ | async as user"
              (click)="logout()"
            >
              Logout
            </button>
          </div>
        </div>
      </nav>
      
    </header>
    <main class="container d-flex flex-column flex-grow-1 h-100 w-100 mt-5">
      <router-outlet></router-outlet>
    </main>
  `,
  styles: [],
})
export class AppComponent {
  constructor(public authService: AuthService) { }

  ngOnInit(): void { }

  logout() {
    this.authService.logout();
  }
}
