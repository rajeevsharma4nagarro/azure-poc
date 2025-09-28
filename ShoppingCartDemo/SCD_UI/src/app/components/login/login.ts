import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../services/user-service';
import { iLoggedInProfile, iUserProfile } from '../../models/iUser.interface';
import { AuthService } from '../../services/auth-service';
import { jwtDecode } from 'jwt-decode';
import { iJwtClaims } from '../../models/iJwtClaims';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  loginForm = {
    email: 'customer@nagarro.com',
    password: 'Pass@123'
  };

  constructor(private router: Router, private userService: UserService, private authService: AuthService) { }

  login() {
    if (this.loginForm.email && this.loginForm.password) {
      this.userService.userLogin({ email: this.loginForm.email, password: this.loginForm.password }).subscribe({
        next: (res: any) => {
          if (res.isSuccess) {
            const claims = jwtDecode<iJwtClaims>(res.result.token);
            let userDetails: iLoggedInProfile = {
              id: claims.sub,
              address: claims.Address,
              city: claims.City,
              email: claims.email,
              fullName: claims.FullName,
              roleName: claims.RoleName,
              state: claims.State,
              zip: claims.Zip
            } 
            

            this.authService.setToken(res.result.token, userDetails);
            this.router.navigate(['/home']);
          } 
          else 
          {
            alert('Please enter email & password');
          }
        },
        error: (err) => {
          console.log(err);
          alert(err.error.message);
        },
        complete: () => {
          console.log('completed');
        }
      
      });
      
    } else {
      alert('Please enter email & password');
    }
  }
}
