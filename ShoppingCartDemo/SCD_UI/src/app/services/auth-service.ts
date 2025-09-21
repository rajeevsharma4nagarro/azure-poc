import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, Subject } from 'rxjs';
import { environment } from '../../environments/environment';
import { iLoggedInProfile, iUser, iUserProfile } from '../models/iUser.interface';
import { GlobalService } from './global.services';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  
  constructor(private http: HttpClient, private globalServices: GlobalService) { }

  login(credentials: { userId: string, password: string }): Observable<any> {
    return this.http.post(`${environment.authApiUrl}/login`, credentials);
    //return of(Mock_Users.filter(x => x.userId == credentials.userId && x.password == credentials.password));
  }

  isLoggedIn(): boolean {
    return this.globalServices.getUserToken() !== null && this.globalServices.getUserToken() !== '';
  }

  setToken(token: string, userDetails: iLoggedInProfile): void {
    this.globalServices.setUserToken(token);
    this.globalServices.setUserProfil(userDetails);
  }

  clearToken(): void {
    this.globalServices.clearUserData();
  }

  getToken(): string| null {
    return this.globalServices.getUserToken();
  }
}
