import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  constructor(private http: HttpClient) {

  }

  getCart(userId: string): Observable<any> {
    return this.http.get<any>(`${environment.cartApiUrl}/cart/GetCart/${userId}`);
  }

  upsertCart(formData: any): Observable<any> {
    return this.http.post<any>(`${environment.cartApiUrl}/cart/CartUpsert`, formData);
  }
}
