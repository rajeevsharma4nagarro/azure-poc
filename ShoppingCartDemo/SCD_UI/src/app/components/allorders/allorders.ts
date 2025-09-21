import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../services/order-service';
import { GlobalService } from '../../services/global.services';
import { iOrders } from '../../models/iOrders';

@Component({
  selector: 'app-allorders',
  standalone: false,
  templateUrl: './allorders.html',
  styleUrl: './allorders.css'
})
export class Allorders {
  orders: iOrders[] = [];
  constructor(private oService: OrderService, private globalServices: GlobalService) {

  }

  ngOnInit() {
    this.bindData();
  }


  updateStatus(orderHeaderId: number, status: boolean) {
    this.oService.updateOrderStatus(orderHeaderId, status).subscribe({
      next: (res) => {
        //this.orders = res.result;
        if (res.isSuccess) {
          this.bindData();
        } else {
          alert(res.message);
        }
      },
      error: () => {

      },
      complete: () => {

      }
    })
  }

  bindData() {
    this.oService.getPendingOrders().subscribe({
      next: (res) => {
        this.orders = res.result;
      },
      error: () => {

      },
      complete: () => {

      }
    });
  }
}
