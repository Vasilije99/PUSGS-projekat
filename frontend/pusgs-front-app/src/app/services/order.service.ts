import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Order, OrderWithId } from '../model/order';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  getAllOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(this.baseUrl + '/Order/orders');
  }

  createNewOrder(newOrder: Order) {
    return this.http.post(this.baseUrl + '/Order/newOrder', newOrder);
  }

  getMyPendingOrder(userId: number): Observable<Order> {
    return this.http.get<Order>(this.baseUrl + '/Order/getMyPendingOrders/' + userId);
  }

  getUserFinishedOrders(userId: number): Observable<OrderWithId[]> {
    return this.http.get<OrderWithId[]>(this.baseUrl + '/Order/getUserFinishedOrders/' + userId);
  }

  getPendingOrders(): Observable<OrderWithId[]> {
    return this.http.get<OrderWithId[]>(this.baseUrl + '/Order/pendingOrders');
  }

  getDelivery(orderId: number,delivererId: number):Observable<OrderWithId> {
    return this.http.put<OrderWithId>(this.baseUrl + '/Order/approveOrder/' + orderId + '/' + delivererId, null);
  }

  getActiveOrder(delivererId: number): Observable<OrderWithId> {
    return this.http.get<OrderWithId>(this.baseUrl + '/Order/getMyActiveOrder/' + delivererId);
  }

  getDelivererFinishedOrders(delivererId: number): Observable<OrderWithId[]> {
    return this.http.get<OrderWithId[]>(this.baseUrl + '/Order/finishedOrders/' + delivererId);
  }

  finishOrder(orderId: number, delivererId: number): Observable<OrderWithId> {
    return this.http.put<OrderWithId>(this.baseUrl + '/Order/finishOrder/' + orderId + '/' + delivererId, null)
  }

  lonLat(address: string) {
    return this.http.get('https://api.geoapify.com/v1/geocode/search?text=' + address + '&apiKey=39147befa79a470f90a3d04fe3d181ea');
  }
}
