import { Component, Input, OnInit } from '@angular/core';
import { OrderWithId } from 'src/app/model/order';
import { OrderProductService } from 'src/app/services/order-product.service';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-pending-orders',
  templateUrl: './pending-orders.component.html',
  styleUrls: ['./pending-orders.component.css']
})
export class PendingOrdersComponent implements OnInit {

  pendingOrders!: OrderWithId[];
  currentOrder!: OrderWithId;
  currentOrderDisplay: boolean = false;
  timerActivate!: boolean;
  @Input() userId!: number;

  constructor(private orderService: OrderService, private orderProductService: OrderProductService) { }

  ngOnInit() {
    this.orderService.getActiveOrder(this.userId).subscribe(
      data => {
        this.currentOrder = data;
      }, error => {
        alert(error);
      }
    )

    this.orderService.getPendingOrders().subscribe(
      data => {
        this.pendingOrders = data;

        this.pendingOrders.forEach((item) => {
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



  takeDelivery(id: number, userId: number): void {
    if(this.currentOrder !== null && this.currentOrder !== undefined) {
      alert("You have active delivery, and you can't take another delivery!");
    } else {
      this.orderService.getDelivery(id, userId).subscribe(
        data => {
          this.currentOrder = data;
          //this.timerActivate = true;
          window.location.reload();
        }, error => {
          alert(error);
        }
      )
    }
  }

  currentOrderClick() {
    if(this.currentOrder === null) {
      alert("You don't have active delivery")
    } else {
      this.currentOrderDisplay = true;
      this.timerActivate = true;
    }
  }

  handleEvent(event : any, id: number) {
    if(event.action === 'done') {
      alert("Delivery is done!");
      this.orderService.finishOrder(id, this.userId).subscribe(
        data => {
          window.location.reload();
        }, error => {
          alert(error);
        }
      )
    }
  }

}
