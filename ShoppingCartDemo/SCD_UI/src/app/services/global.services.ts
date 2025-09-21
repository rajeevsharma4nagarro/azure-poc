import { BehaviorSubject, Observable, of, Subject } from "rxjs";
import { Injectable } from "@angular/core";
import { iLoggedInProfile, iUser, iUserProfile } from "../models/iUser.interface";

@Injectable({
  providedIn: 'root'
})
export class GlobalService {
  private subjectUserProfile = new BehaviorSubject<iLoggedInProfile | null>(null);
  userProfile$ = this.subjectUserProfile.asObservable();

  setUserProfil(user: iLoggedInProfile | null) {
    this.subjectUserProfile.next(user);
  }

  getUserProfil() {
    return this.subjectUserProfile.value;
  }

  private subjectToken = new BehaviorSubject<string | null>(null);
  userToken$ = this.subjectToken.asObservable();

  setUserToken(token: string | null) {
    this.subjectToken.next(token);
  }

  getUserToken() {
    return this.subjectToken.value;
  }

  private cartItems = new BehaviorSubject<CartItem[] | null>([]);
  cartItems$ = this.cartItems.asObservable();

  setCartItems(cItems: CartItem[] | null) {
    this.cartItems.next(cItems);
  }

  getCartItems() {
    return this.cartItems.value;
  }

  clearUserData() {
    this.subjectUserProfile.next(null);
  }
}


export enum ProductCategory {
  Clothing ='Clothing',
  Footwear ='Footwear',
  Accessories ='Accessories',
  Electronics ='Electronics'
}

export const MockProducts: iProduct[] = [
  { id: 1, name: 'T-shirt', category: 'Clothing', price: 499, description: '', imageUrl: 'assets/images/t-shirt.png' },
  { id: 2, name: 'Jeans', category: 'Clothing', price: 1199, description: '', imageUrl: 'assets/images/jeams.png' },
  { id: 3, name: 'Shoes', category: 'Footwear', price: 1999, description: '', imageUrl: 'assets/images/shoose.png' },
  { id: 4, name: 'Watch', category: 'Accessories', price: 1599, description: '', imageUrl: 'assets/images/watch.png' },
  { id: 5, name: 'Headphones', category: 'Electronics', price: 2499, description: '', imageUrl: 'assets/images/headphone.png' },
  { id: 6, name: 'Laptop Bag', category: 'Accessories', price: 899, description: '', imageUrl: 'assets/images/laptop.png' },
  { id: 7, name: 'Jacket', category: 'Clothing', price: 1499, description: '', imageUrl: 'assets/images/jacket.png' },
  { id: 8, name: 'Smartphone', category: 'Electronics', price: 15999, description: '', imageUrl: 'assets/images/bag.png' },
  { id: 9, name: 'Cap', category: 'Accessories', price: 299, description: '', imageUrl: 'assets/images/cap.png' },
  { id: 10, name: 'Sandals', category: 'Footwear', price: 799, description: '', imageUrl: 'assets/images/sandle.png' },
];
