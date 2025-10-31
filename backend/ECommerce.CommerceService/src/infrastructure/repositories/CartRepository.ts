import { ICartRepository } from '../../application/interfaces/ICartRepository';
import { Cart } from '../../application/entities/Cart';
import { CartModel } from '../persistence/models/CartModel';

export class CartRepository implements ICartRepository {
  async findByUserId(userId: string): Promise<Cart | null> {
    const doc = await CartModel.findOne({ userId }).exec();
    return doc ? doc.toObject() : null;
  }

  async findById(id: string): Promise<Cart | null> {
    const doc = await CartModel.findById(id).exec();
    return doc ? doc.toObject() : null;
  }

  async create(cart: Cart): Promise<Cart> {
    const doc = new CartModel(cart);
    await doc.save();
    return doc.toObject();
  }

  async update(id: string, cart: Partial<Cart>): Promise<Cart | null> {
    cart.updatedAt = new Date();
    // Calculate totalAmount if items updated
    if (cart.items) {
      cart.totalAmount = cart.items.reduce((sum, item) => sum + (item.price * item.quantity), 0);
    }
    const doc = await CartModel.findByIdAndUpdate(id, cart, { new: true }).exec();
    return doc ? doc.toObject() : null;
  }

  async delete(id: string): Promise<boolean> {
    const result = await CartModel.findByIdAndDelete(id).exec();
    return result !== null;
  }

  async deleteByUserId(userId: string): Promise<boolean> {
    const result = await CartModel.findOneAndDelete({ userId }).exec();
    return result !== null;
  }
}



