import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Order } from '../model/order';

@Injectable({
  providedIn: 'root'
})
export class OrderProductService {

  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  createNewOrderProduct(map: {}, orderId: number) {
    return this.http.post(this.baseUrl + '/OrderProduct/newOrderProduct/' + orderId, map);
  }

  getOrderDetails(orderId: number) {
    return this.http.get(this.baseUrl + '/OrderProduct/orderDetails/' + orderId);
  }
}
