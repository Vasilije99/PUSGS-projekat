import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Order, OrderProduct } from 'src/app/model/order';
import { Product } from 'src/app/model/product';
import { OrderProductService } from 'src/app/services/order-product.service';
import { OrderService } from 'src/app/services/order.service';
import { ProductService } from 'src/app/services/product.service';
import { UserOrderService } from 'src/app/services/user-order.service';

declare let paypal: any;

@Component({
  selector: 'app-create-order',
  templateUrl: './create-order.component.html',
  styleUrls: ['./create-order.component.css']
})
export class CreateOrderComponent implements OnInit {

  @Input() userId!: number;

  allProducts!: Product[];
  orderProducts: OrderProduct[] = [];
  newOrderForm!: FormGroup;
  orderPrice: number = 0;
  currentOrder!: Order;
  paypal: any;
  addScript: boolean = false;

  constructor(
    private productService: ProductService,
    private fb: FormBuilder,
    private orderService: OrderService,
    private orderProductService: OrderProductService,
    private userOrderService: UserOrderService) {}

  ngOnInit() {
    this.orderService.getMyPendingOrder(this.userId).subscribe(
      data => {
        this.currentOrder = data;
      }, error => {
        console.log('Ne postoji trenutna porudzbina')
      }
    )

    if(this.currentOrder === undefined) {
      this.productService.getAllProducts().subscribe(
        data => {
          this.allProducts = data;

          for(let i = 0; i < this.allProducts.length; i++) {
            this.orderProducts.push({productName: this.allProducts[i].name, price: this.allProducts[i].price, ingredients: this.allProducts[i].ingredients, count: 0});
          }
        }, error => {
          alert(error);
        }
      )

      this.createNewOrderForm();
    }
  }

  paypalConfig = {
    env: 'sandbox',
    client: {
      sandbox: 'AVRYW8PsEVx1mt4V7GBXcHHhUCnQyatrGJffvMmPJekR1QURipkCxYywPrQDEFiWunLSu4VcTdeuqGKF',
      production: 'EKYI-pyoEHDMlIQIOZkE4oKmlNgrMXHOPHydz-_SzA6X0fuHnBj_Mbu19tA6QKaRsvOaDovI8MMEN8lO'
    },
    commit: true,
    payment: (data: any, actions: any) => {
      return actions.payment.create({
        payment: {
          transactions: [
            {amount: {total: 100, currency: 'USD'}}
          ]
        }
      });
    },
    onAuthorize: (data:any, actions:any) => {
      return actions.payment.execute().then((payment:any) => {
        this.onSubmit();
      })
    }
  }

  ngAfterViewChecked(): void {
    if(!this.addScript){
      this.addPaypalScript().then(() => {
        paypal.Button.render(this.paypalConfig, '#paypal-checkout-btn');
      })
    }
  }

  addPaypalScript(){
    this.addScript = true;
    return new Promise((resolve, reject) => {
      let scripttagElement = document.createElement('script');
      scripttagElement.src = 'http://www.paypalobjects.com/api/checkout.js';
      scripttagElement.onload = resolve;
      document.body.appendChild(scripttagElement);
    })
  }

  createNewOrderForm() {
    this.newOrderForm = this.fb.group({
      address: [null, Validators.required],
      comment: [null, Validators.required]
    })
  }

  onOrderChange(productName: string, corrector: string) {
    let temp: number = 0;

    this.orderProducts.forEach(function (item) {
      if(item.productName === productName) {
        //corrector === '+' ? item.count = item.count + 1 : (item.count > 0 ? item.count - 1 : 0);

        if(corrector === '+') {
          item.count = item.count + 1;
          temp = temp + (item.count * item.price);
        } else {
          if(item.count > 0) {
            item.count = item.count - 1
            temp = temp + (item.count * item.price);
          }
        }
      } else {
        temp = temp + (item.count * item.price);
      }
    })

    this.orderPrice = temp;
  }

  onSubmit() {
    let coordinates: any;
    let longitude: number = 0;
    let latitude: number = 0;

    this.orderService.lonLat(this.address.value).subscribe(
      data => {
        coordinates = data;
        longitude = coordinates.features[0].geometry.coordinates[0];
        latitude = coordinates.features[0].geometry.coordinates[1];

        this.payWithCash(longitude, latitude);
      }
    )
  }

  payWithCash(longitude: number, latitude: number) {
    const obj = [...this.createMap()].reduce((o, [key, value]) => ((o as any)[key] = value, o), {});

    if(this.newOrderForm.valid) {
      this.orderService.createNewOrder({deliveryAddress: this.address.value, comment: this.comment.value, price: this.orderPrice, orderState: 0, lat: latitude, lon: longitude}).subscribe(
        data => {
          let orderId = Number(data);
          this.userOrderService.createNewUserOrder(this.userId, orderId).subscribe(
            data => {
              this.orderProductService.createNewOrderProduct(obj, orderId).subscribe(
                data => {
                  alert("Order is successfully created!")
                  window.location.reload();
                }, error => {
                  alert(error);
                }
              )
            }, error => {
              alert(error)
            }
          )
        }, error => {
          alert(error);
        }
      )
    } else {
      alert("Some fields are not filled as well")
    }
  }

  createMap() {
    let map = new Map<string, number>();
    for(var item of this.orderProducts) {
      if(item.count > 0) {
        map.set(item.productName, item.count)
      }
    }

    return map;
  }

  get address() {
    return this.newOrderForm.get('address') as FormControl;
  }

  get comment() {
    return this.newOrderForm.get('comment') as FormControl;
  }

}
