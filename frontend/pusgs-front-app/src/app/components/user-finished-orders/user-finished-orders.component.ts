import { Component, Input, OnInit } from '@angular/core';
import { Order, OrderWithId } from 'src/app/model/order';
import { OrderProductService } from 'src/app/services/order-product.service';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-user-finished-orders',
  templateUrl: './user-finished-orders.component.html',
  styleUrls: ['./user-finished-orders.component.css']
})
export class UserFinishedOrdersComponent implements OnInit {

  finishedOrders!: OrderWithId[];
  @Input() userId!: number;

  constructor(private orderService: OrderService, private orderProductService: OrderProductService) { }

  ngOnInit() {
    this.orderService.getUserFinishedOrders(this.userId).subscribe(
      data => {
        this.finishedOrders = data;

        this.finishedOrders.forEach((item) => {
          if(item !== null) {
            this.orderProductService.getOrderDetails(item?.id).subscribe(
              data => {
                item.product = JSON.stringify(data).slice(1,-1);
              }, error => {
                alert(error);
              }
            )
          }
        })
      }, error => {
        alert(error);
      }
    )
  }

}
