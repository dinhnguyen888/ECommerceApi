import { ICartRepository } from '../../application/interfaces/ICartRepository';
import { Cart } from '../../application/entities/Cart';
import { CartModel } from '../persistence/models/CartModel';

export class CartRepository implements ICartRepository {
  private toCart(doc: any): Cart {
    if (!doc) return null as any;
    const obj = doc.toObject ? doc.toObject() : doc;
    return {
      ...obj,
      _id: obj._id ? obj._id.toString() : undefined
    };
  }

  async findByUserId(userId: string): Promise<Cart | null> {
    const doc = await CartModel.findOne({ userId }).exec();
    return doc ? this.toCart(doc) : null;
  }

  async findById(id: string): Promise<Cart | null> {
    const doc = await CartModel.findById(id).exec();
    return doc ? this.toCart(doc) : null;
  }

  async create(cart: Cart): Promise<Cart> {
    const doc = new CartModel(cart);
    await doc.save();
    return this.toCart(doc);
  }

  async update(id: string, cart: Partial<Cart>): Promise<Cart | null> {
    cart.updatedAt = new Date();
    // Tinh toan totalAmount neu items duoc cap nhat
    if (cart.items) {
      cart.totalAmount = cart.items.reduce((sum, item) => sum + (item.price * item.quantity), 0);
    }
    const doc = await CartModel.findByIdAndUpdate(id, cart, { new: true }).exec();
    return doc ? this.toCart(doc) : null;
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



