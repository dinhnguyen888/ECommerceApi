export interface CartItemDto {
  productId: string;
  quantity: number;
  price: number;
}

export interface CartCreateDto {
  userId: string;
  items: CartItemDto[];
}

export interface CartUpdateDto {
  items?: CartItemDto[];
}

export interface CartResponseDto {
  _id: string;
  userId: string;
  items: CartItemDto[];
  totalAmount: number;
  createdAt?: Date;
  updatedAt?: Date;
}



