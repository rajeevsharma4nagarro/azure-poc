import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  constructor(private http: HttpClient) {

  }

  createOrder(formData: any): Observable<any> {
    return this.http.post<any>(`${environment.orderApiUrl}/checkout/CreateOrder`, formData);
  }

  getOrders(userId: string): Observable<any> {
    return this.http.get<any>(`${environment.orderApiUrl}/checkout/GetOrders/${userId}`);
  }

  getPendingOrders(): Observable<any> {
    return this.http.get<any>(`${environment.orderApiUrl}/checkout/GetPendingOrders`);
  }

  updateOrderStatus(oId: number, approved: boolean): Observable<any> {
    return this.http.post<any>(`${environment.orderApiUrl}/checkout/updateOrderStatus`, { orderHeaderId: oId, isApproved: approved });
  }
}
