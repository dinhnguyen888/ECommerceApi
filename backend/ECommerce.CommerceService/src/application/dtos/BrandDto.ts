export interface BrandCreateDto {
  name: string;
  description?: string;
  logo?: string;
}

export interface BrandUpdateDto {
  name?: string;
  description?: string;
  logo?: string;
}

export interface BrandResponseDto {
  _id: string;
  name: string;
  description?: string;
  logo?: string;
  createdAt?: Date;
  updatedAt?: Date;
}



