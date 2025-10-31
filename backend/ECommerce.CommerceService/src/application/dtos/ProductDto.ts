export interface ProductCreateDto {
  name: string;
  description: string;
  price: number;
  stock: number;
  brandId: string;
  categoryId: string;
  images?: string[];
}

export interface ProductUpdateDto {
  name?: string;
  description?: string;
  price?: number;
  stock?: number;
  brandId?: string;
  categoryId?: string;
  images?: string[];
}

export interface ProductResponseDto {
  _id: string;
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



