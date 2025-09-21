export interface CartHeaderDto {
  cartHeaderId: number;
  userId: string;
  cartTotal: number;
}
export interface CartDetailsDto {
  cartDetailId: number;
  cartHeaderId: number;
  productId: number;
  price: number;
  count: number;
}
export interface CartDto {
  cartHeader: CartHeaderDto;
  cartDetails: CartDetailsDto[];
}
