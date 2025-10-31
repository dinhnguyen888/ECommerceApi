import { Cart } from '../entities/Cart';

export interface ICartRepository {
  findByUserId(userId: string): Promise<Cart | null>;
  findById(id: string): Promise<Cart | null>;
  create(cart: Cart): Promise<Cart>;
  update(id: string, cart: Partial<Cart>): Promise<Cart | null>;
  delete(id: string): Promise<boolean>;
  deleteByUserId(userId: string): Promise<boolean>;
}



