import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { GlobalService } from '../../../services/global.services';
import { CartService } from '../../../services/cart-service';
import { CartStateService } from './CartStateService/cart-state-service';

@Component({
  selector: 'app-cart-items',
  standalone: false,
  templateUrl: './cart-items.html',
  styleUrl: './cart-items.css'
})
export class CartItems implements OnInit {
  cartItems: CartItem[] = [];
  @Output('cartTotal') cartTotal = new EventEmitter<number>();
  @Input('viewType') viewType: 'Small' | 'Full' = 'Small';
  
  constructor(private globalServices: GlobalService, private cart: CartService,
    private cartState: CartStateService) {

  }

  ngOnInit() {
    this.cartState.items$.subscribe(items => {
      this.cartItems = items;
      this.calculateTotal();
    });

    this.globalServices.userProfile$.subscribe({
      next: (profile: any) => {
        if (profile) {
          this.populateCart(profile.id)
        }
        console.log('request success.');
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {
        console.log('request completed.');
      }
    });
  }

  populateCart(userId: string) {
    this.cart.getCart(userId).subscribe({
      next: (resp) => {
        let result = resp.result;
        if (resp.isSuccess && result && result.cartDetails) {
          let tempCartItems: CartItem[] = [];
          result.cartDetails.forEach((i: any) => {
            tempCartItems.push({
              id: i.product.id,
              category: i.product.category,
              description: i.product.description,
              imageUrl: i.product.imageUrl,
              name: i.product.name,
              price: i.product.price,
              qty: i.count
            });
          });
          this.cartState.setItems(tempCartItems);
        }
      },
      error: (err) => {

      },
      complete: () => {
        
      }
    });
  }

  // Increase quantity
  increaseQty(item: CartItem) {
    this.cartState.increaseItemQty(item.id, item.qty + 1);
    this.calculateTotal();
  }

  // Decrease quantity
  decreaseQty(item: CartItem) {
    if (item.qty > 1) {
      this.cartState.decreaseItemQty(item.id, item.qty - 1);
      this.calculateTotal();
    }
  }

  // Remove cart items
  removeFromCart(item: CartItem) {
    this.cartState.removeItem(item.id);
    this.calculateTotal();
  }

  calculateTotal() {
    let total = 0;
    this.cartItems.forEach(i => {
      total += (i.qty * i.price);
    });
    this.cartTotal.emit(total);
  }
}
