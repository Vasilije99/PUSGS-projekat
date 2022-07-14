export interface OrderProduct {
  productName: string;
  price: number;
  ingredients: string;
  count: number;
}

export class Order {
  deliveryAddress: string = '';
  comment: string = '';
  price: number = 0;
  orderState: number = 0;
  lat: number = 0;
  lon: number = 0;
}

export class OrderWithId {
  id: number = -1;
  deliveryAddress: string = '';
  comment: string = '';
  price: number = 0;
  orderState: number = 0;
  lat: number = 0;
  lon: number = 0;
  product: string = '';
}
