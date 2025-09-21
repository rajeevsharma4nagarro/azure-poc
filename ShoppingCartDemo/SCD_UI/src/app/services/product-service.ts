import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  constructor(private http: HttpClient) {

  }

  getAllProducts(): Observable<any> {
    return this.http.get<any>(`${environment.productApiUrl}/product`);
  }

  addProduct(formData: any): Observable<any> {
    return this.http.post<any>(`${environment.productApiUrl}/product`, formData);
  }
}
