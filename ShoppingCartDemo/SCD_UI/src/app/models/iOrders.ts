export interface iOrders {
  orderHeaderId: number,
  orderTotal: number,
  email: string,
  name: string,
  address: string,
  city: string,
  state: string,
  zip: string,
  payment: string,
  status: string,
  orderOn: Date,
  orderDetails: any[]
}
