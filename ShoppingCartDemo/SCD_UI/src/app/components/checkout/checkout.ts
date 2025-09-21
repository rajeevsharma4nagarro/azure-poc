import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { GlobalService } from '../../services/global.services';
import { CartService } from '../../services/cart-service';
import { iLoggedInProfile } from '../../models/iUser.interface';
import { ProductService } from '../../services/product-service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { OrderService } from '../../services/order-service';
import { CartStateService } from './cart-items/CartStateService/cart-state-service';

@Component({
  selector: 'app-checkout',
  standalone: false,
  templateUrl: './checkout.html',
  styleUrl: './checkout.css'
})
export class Checkout implements OnInit {
  cartTotal: number = 0;
  addressForm: FormGroup;
  constructor(private globalServices: GlobalService, private orderService: OrderService, private cd: ChangeDetectorRef,
    private fb: FormBuilder, private router: Router, private cartStateService: CartStateService) {
    this.addressForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', Validators.required],
      address: ['', Validators.required],
      city: ['', Validators.required],
      state: ['', Validators.required],
      zip: ['', Validators.required],
      payment: { value: 'cod', disabled: true },
      cardNumber: [''],
      expiry: [''],
      cvv: [''],
      userId: ['']
    });
  }

  ngOnInit() {
    this.populateAddress();
  }

  populateAddress() {
    this.globalServices.userProfile$.subscribe({
      next: (profile: any) => {
        if (profile) {
          this.addressForm.controls["userId"].patchValue(profile.id);
          this.addressForm.controls["address"].patchValue(profile.address);
          this.addressForm.controls["city"].patchValue(profile.city);
          this.addressForm.controls["state"].patchValue(profile.state);
          this.addressForm.controls["zip"].patchValue(profile.zip);
          this.addressForm.controls["email"].patchValue(profile.email);
          this.addressForm.controls["name"].patchValue(profile.fullName);
        }
        console.log('request success.');
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {
        console.log('request completed.');
      }
    });
  }

  populateCarttotal(total: number) {
    setTimeout(() => {
      this.cartTotal = total;
      //this.cd.detectChanges();
    }, 1);
  }

  onSubmit() {
    if (this.addressForm.valid) {
      let formData = { ...this.addressForm.value, payment: this.addressForm.getRawValue().payment };

      this.orderService.createOrder(formData).subscribe({
        next: (resp) => {

          this.cartStateService.setItems([]);
          console.log('Order placed.');
          
          this.router.navigate(['/confirm', resp.result.orderHeaderId]);
          
          //alert("Order placed.");
        },
        error: (err) => {
          alert(err)
        },
        complete: () => {

        }
      });
    }
    else {

    }
  }
}
