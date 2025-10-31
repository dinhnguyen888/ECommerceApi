export interface CartItem {
  productId: string;
  quantity: number;
  price: number;
}

export interface Cart {
  _id?: string;
  userId: string;
  items: CartItem[];
  totalAmount: number;
  createdAt?: Date;
  updatedAt?: Date;
}



