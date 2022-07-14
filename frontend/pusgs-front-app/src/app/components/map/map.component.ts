import { Component, HostListener, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import * as L from 'leaflet';
import { OrderWithId } from 'src/app/model/order';
import { OrderService } from 'src/app/services/order.service';

const iconRetinaUrl = 'assets/marker-icon-2x.png';
const iconUrl = 'assets/marker-icon.png';
const shadowUrl = 'assets/marker-shadow.png';
const iconDefault = L.icon({
  iconRetinaUrl,
  iconUrl,
  shadowUrl
});
L.Marker.prototype.options.icon = iconDefault;

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['../../../../node_modules/leaflet/dist/leaflet.css']
})
export class MapComponent implements OnInit {

  map!: L.Map;
  innerHeight: any;
  containerHeight!:number;
  icon:any;
  pendingOrders!: OrderWithId[];
  @Input() takeDelivery!: (id: number, userId: number) => void;
  @Input() userId!: number;

  constructor(private orderService: OrderService, private router: Router) { }

  ngOnInit() {
    this.initMap();
    this.defineIcons();
    this.getPendingOrders();
  }

  getPendingOrders() {
    this.orderService.getPendingOrders().subscribe(
      data => {
        this.pendingOrders = data;
        this.addOrderMarkers(this.pendingOrders)
      }
    )
  }

  defineIcons() {
    let iconUrl = '../../../assets/images/icons8-google-maps-old.svg';
    this.icon = L.icon({
      iconUrl
    });
  }

  initMap() {
    this.map = L.map('map', {
      center: [ 45.2396, 19.8227],
      zoom: 14
    });

    const tiles = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png');
    tiles.addTo(this.map);
  }

  addOrderMarkers(orders:OrderWithId[]){
    console.log(orders);
    orders.forEach(element => {

      let marker = L.marker([element.lat, element.lon], {icon:this.icon});
      marker.addTo(this.map);
      marker.on('click', (e) => {
        this.takeDelivery(element.id, this.userId);
      });
    });
  }
}
