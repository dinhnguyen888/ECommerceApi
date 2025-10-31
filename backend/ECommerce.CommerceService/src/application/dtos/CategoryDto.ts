export interface CategoryCreateDto {
  name: string;
  description?: string;
  parentId?: string;
}

export interface CategoryUpdateDto {
  name?: string;
  description?: string;
  parentId?: string;
}

export interface CategoryResponseDto {
  _id: string;
  name: string;
  description?: string;
  parentId?: string;
  createdAt?: Date;
  updatedAt?: Date;
}



