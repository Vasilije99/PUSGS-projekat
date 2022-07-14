import { Component, Input, OnInit } from '@angular/core';
import { OrderWithId } from 'src/app/model/order';
import { OrderProductService } from 'src/app/services/order-product.service';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-deliverer-orders',
  templateUrl: './deliverer-orders.component.html',
  styleUrls: ['./deliverer-orders.component.css']
})
export class DelivererOrdersComponent implements OnInit {

  myFinishedOrders!: OrderWithId[];
  @Input() userId!: number;

  constructor(private orderService: OrderService, private orderProductService: OrderProductService) { }

  ngOnInit() {
    this.orderService.getDelivererFinishedOrders(this.userId).subscribe(
      data => {
        this.myFinishedOrders = data;

        this.myFinishedOrders.forEach((item) => {
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
        console.log(error);
      }
    )
  }
}
