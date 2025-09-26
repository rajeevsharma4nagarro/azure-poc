import { Component, OnInit } from '@angular/core';
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
export class Signup  implements OnInit {
  signupForm: FormGroup;

  constructor(private router: Router, private fb: FormBuilder, private service: UserService) {
    this.signupForm = new FormGroup({
      rolename: new FormControl<string>('', { nonNullable: true, validators: [Validators.required] }),
      name: new FormControl<string>('new user name', { nonNullable: true, validators: [Validators.required] }),
      email: new FormControl<string>('user@nagarro.com', { nonNullable: true, validators: [Validators.required, Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$')] }),
      password: new FormControl<string>('Pass@123', { nonNullable: true, validators: [Validators.required, Validators.pattern('^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$')] }),
      cpassword: new FormControl<string>('Pass@123', { nonNullable: true, validators: Validators.required }),
      address: new FormControl<string>('Plot 6B, Sub. Major Laxmi Chand Rd, Maruti Udyog, Sector 18, Sarhol', { nonNullable: true, validators: Validators.required }),
      city: new FormControl<string>('Gurugram', { nonNullable: true, validators: Validators.required }),
      state: new FormControl<string>('Haryana', { nonNullable: true, validators: Validators.required }),
      zip: new FormControl<string>('122015', { nonNullable: true, validators: [Validators.required, Validators.pattern('^[1-9]{1}[0-9]{5}$')] }),
    }, { validators: passwordMatchValidator('password', 'cpassword') });
  }

  ngOnInit(): void {
    
  }

  isInvalid(controlName: string): boolean {
    const control = this.signupForm.get(controlName);
    if(!control) return true;

    return control?.invalid && control?.touched;
  }

  onSubmit(): void {
    if (this.signupForm.valid) {
      console.log('Form submitted successfully!');
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
          else{
            this.signupForm.get('email')?.setErrors({ duplicateemail: true });
            this.signupForm.markAllAsTouched();
            
          }
          console.log(resp);
        },
        error: (err) => {
          console.log(err);
        },
        complete: () => {
          console.log('Respone end.');
        }
      });

    } else {
      console.log(this.signupForm.get('password'));
      this.signupForm.markAllAsTouched();
      console.log('Form has validation errors');
    }
  }
}
