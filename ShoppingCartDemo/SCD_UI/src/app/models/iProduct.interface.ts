interface iProduct {
  id: number;
  name: string;
  price: number;
  description: string;
  category: string;
  imageUrl: string;
}
interface CartItem extends iProduct {
  qty: number;
}
