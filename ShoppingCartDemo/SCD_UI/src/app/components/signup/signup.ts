import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { passwordMatchValidator } from '../../shared/shared-functions';
import { iRegistrationRequest } from '../../models/iUser.interface';
import { UserService } from '../../services/user-service';

@Component({
  selector: 'app-signup',
  standalone: false,
  templateUrl: './signup.html',
  styleUrl: './signup.css'
})
export class Signup {
  signupForm: FormGroup;

  constructor(private router: Router, private fb: FormBuilder, private service: UserService) {
    this.signupForm = new FormGroup({
      rolename: new FormControl<string>('', { nonNullable: true, validators: [Validators.required] }),
      name: new FormControl<string>('', { nonNullable: true, validators: [Validators.required] }),
      email: new FormControl<string>('', { nonNullable: true, validators: [Validators.required, Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$')] }),
      password: new FormControl<string>('', { nonNullable: true, validators: [Validators.required, Validators.pattern('^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$')] }),
      cpassword: new FormControl<string>('', { nonNullable: true, validators: Validators.required }),
      address: new FormControl<string>('', { nonNullable: true, validators: Validators.required }),
      city: new FormControl<string>('', { nonNullable: true, validators: Validators.required }),
      state: new FormControl<string>('', { nonNullable: true, validators: Validators.required }),
      zip: new FormControl<string>('', { nonNullable: true, validators: [Validators.required, Validators.pattern('^[1-9]{1}[0-9]{5}$')] }),
    }, { validators: passwordMatchValidator('password', 'cpassword') });
  }

  onSubmit(): void {
    if (this.signupForm.valid) {
      let request: iRegistrationRequest = {
        email: this.signupForm.get('email')?.value,
        fullName: this.signupForm.get('name')?.value,
        roleName: this.signupForm.get('rolename')?.value,
        password: this.signupForm.get('password')?.value,
        address: this.signupForm.get('address')?.value,
        city: this.signupForm.get('city')?.value,
        state: this.signupForm.get('state')?.value,
        zip: this.signupForm.get('zip')?.value,
      }

      this.service.createUser(request).subscribe({
        next: (resp: any) => {
          if (resp.isSuccess) {
            alert('Account created, please login.')
            this.router.navigate(['/']); // Redirect to login
          }
          console.log(resp);
        },
        error: (err) => {
          console.log(err);
        },
        complete: () => {

        }
      });

    } else {
      
    }
  }
}
