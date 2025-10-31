export interface Product {
  _id?: string;
  name: string;
  description: string;
  price: number;
  stock: number;
  brandId: string;
  categoryId: string;
  images?: string[];
  createdAt?: Date;
  updatedAt?: Date;
}



