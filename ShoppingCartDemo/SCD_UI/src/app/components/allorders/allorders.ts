import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../services/order-service';
import { GlobalService } from '../../services/global.services';
import { iOrders } from '../../models/iOrders';
import { LoadingService } from '../../services/loading-service';

@Component({
  selector: 'app-allorders',
  standalone: false,
  templateUrl: './allorders.html',
  styleUrl: './allorders.css'
})
export class Allorders {
  orders: iOrders[] = [];
  constructor(private oService: OrderService, private globalServices: GlobalService, private loader: LoadingService) {

  }

  ngOnInit() {
    this.bindData();
  }


  updateStatus(orderHeaderId: number, status: boolean) {
    this.loader.show();
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
        this.loader.hide();
      }
    })
  }

  bindData() {
    this.loader.show();
    this.oService.getPendingOrders().subscribe({
      next: (res) => {
        this.orders = res.result;
      },
      error: () => {

      },
      complete: () => {
        this.loader.hide();
      }
    });
  }
}
