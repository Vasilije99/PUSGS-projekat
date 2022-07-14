import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserOrderService {

  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  createNewUserOrder(userId: number, orderId: number) {
    return this.http.post(this.baseUrl + '/UserOrder/newUserOrder/' + userId + '/' + orderId, null);
  }
}
