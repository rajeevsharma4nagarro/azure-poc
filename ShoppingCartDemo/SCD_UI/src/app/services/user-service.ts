import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { iRegistrationRequest, iUser } from '../models/iUser.interface';
import { AuthService } from './auth-service';
import { environment } from '../../environments/environment';
import { RoleTypes } from '../shared/shared-enums';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  
  constructor(private http: HttpClient, private auth: AuthService) {  }

  createUser(value: iRegistrationRequest): Observable<any> {
    return this.http.post<any>(`${environment.authApiUrl }/auth/register`, value);
    //return of();
  }

  userLogin(value: { email: string, password: string }): Observable<any> {
    return this.http.post<any>(`${environment.authApiUrl}/auth/login`, value);
  }
}
