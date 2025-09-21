import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../services/order-service';
import { GlobalService } from '../../services/global.services';
import { iOrders } from '../../models/iOrders';

@Component({
  selector: 'app-myorders',
  standalone: false,
  templateUrl: './myorders.html',
  styleUrl: './myorders.css'
})
export class Myorders implements OnInit {
  orders: iOrders[] = [];
  constructor(private oService: OrderService, private globalServices: GlobalService) {

  }

  ngOnInit() {
    this.globalServices.userProfile$.subscribe({
      next: (profile: any) => {
        if (profile) {
          this.getOrders(profile.id)
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

  getOrders(userId: string) {
    this.oService.getOrders(userId).subscribe({
      next: (res) => {
        this.orders = res.result;
        
      },
      error: () => {

      },
      complete: () => {

      }
    })
  }
}
