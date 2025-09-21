import { Component, OnInit, signal } from '@angular/core';
import { GlobalService } from './services/global.services';
import { Router } from '@angular/router';
import { AuthService } from './services/auth-service';
import { CartStateService } from './components/checkout/cart-items/CartStateService/cart-state-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})
export class App implements OnInit {
  userName = '';
  roleName = ''
  isLoggedIn = false;
  cartTotal: number = 0;
  constructor(private globalServices: GlobalService, private router: Router, private service: AuthService, private stateService: CartStateService) {
    
  }

  ngOnInit() {
    this.globalServices.userProfile$.subscribe({
      next: (profile: any) => {
        this.userName = profile?.email;
        this.roleName = profile?.roleName;
      },
      error: () => { },
      complete: () => { }
    });

    this.globalServices.userToken$.subscribe({
      next: (token: any) => {
        this.isLoggedIn = token ? true : false;
      },
      error: () => { },
      complete: () => { }
    });
  }

  populateCarttotal(total: number) {
    setTimeout(() => {
      this.cartTotal = total;
    }, 1);
  }

  logout() {
    this.globalServices.clearUserData();
    this.service.clearToken();
    this.isLoggedIn = false;
    this.stateService.setItems([]);
    this.router.navigate(['/']);
  }
}
