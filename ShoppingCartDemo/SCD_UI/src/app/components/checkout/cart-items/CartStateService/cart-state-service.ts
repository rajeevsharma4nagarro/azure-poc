import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { GlobalService } from '../../../../services/global.services';
import { CartHeaderDto, CartDto, CartDetailsDto } from '../../../../models/iCart';
import { CartService } from '../../../../services/cart-service';
interface actionOnItem {
  id: number;
  status: -1 | 0 | 1;
}

@Injectable({
  providedIn: 'root'
})
export class CartStateService {
  private _items = new BehaviorSubject<CartItem[]>([]);
  items$ = this._items.asObservable();

  constructor(private globalServices: GlobalService, private cartService: CartService) {

  }
  get items(): CartItem[] {
    return this._items.value;
  }

  setItems(items: CartItem[]) {
    this._items.next(items);
  }

  addItem(item: CartItem) {
    let updated: CartItem[] = []
    if (this.items.filter(i => i.id == item.id).length > 0) {
      const qty = this.items.filter(i => i.id == item.id)[0].qty + 1;
      updated = this.items.map(i => i.id === item.id ? { ...i, qty } : i);
    }
    else {
      updated = [...this.items, item];
    }
    //this._items.next(updated);
    this.upsert_CartItems(updated, { id: item.id, status: 1 });
  }

  increaseItemQty(id: number, qty: number) {
    const updated = this.items.map(i => i.id === id ? { ...i, qty } : i);
    //this._items.next(updated);
    this.upsert_CartItems(updated, { id: id, status: 1 });
  }

  decreaseItemQty(id: number, qty: number) {
    const updated = this.items.map(i => i.id === id ? { ...i, qty } : i);
    //this._items.next(updated);
    this.upsert_CartItems(updated, { id: id, status: -1 });
  }

  removeItem(id: number) {
    const updated = this.items.filter(i => i.id !== id);
    //this._items.next(updated);
    this.upsert_CartItems(updated, { id: id, status: 0 });
  }

  upsert_CartItems(updated: CartItem[], actionItem: actionOnItem) {
    let userName = '';
    let userId = '';
    let roleName = '';
    this.globalServices.userProfile$.subscribe({
      next: (profile: any) => {
        userName = profile?.email;
        userId = profile?.id;
        roleName = profile?.roleName;

        const tempitem = updated.filter(i => i.id == actionItem.id)[0];

        let cartDto: CartDto = {
          cartHeader: {
            cartHeaderId: 0,
            cartTotal: 0,
            userId: userId
          },
          cartDetails: []
        }
        
        let cartDetails: CartDetailsDto = {
          cartDetailId: 0,
          cartHeaderId: 0,
          count: tempitem? tempitem.qty : 0,
          price: tempitem? tempitem.price : 0,
          productId: actionItem.id
        };
        cartDto.cartDetails.push(cartDetails);

        this.cartService.upsertCart(cartDto).subscribe({
          next: (resp) => {
            //alert('next');
          },
          error: (err) => {
            //alert('error');
          },
          complete: () => {
            //alert('completed');
            this._items.next(updated);
            //alert(`Cart updated!`);
          }
        })
      },
      error: (err) => {
        alert(err);
      },
      complete: () => {
       
      }
    });
  }
}
