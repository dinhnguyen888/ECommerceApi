import { ICartRepository } from '../interfaces/ICartRepository';
import {
  CartCreateDto,
  CartUpdateDto,
  CartResponseDto,
  CartItemDto
} from '../dtos/CartDto';
import { Cart } from '../entities/Cart';

export class CartService {
  constructor(private cartRepository: ICartRepository) {}

  async getByUserId(userId: string): Promise<CartResponseDto | null> {
    const cart = await this.cartRepository.findByUserId(userId);
    return cart ? this.toResponseDto(cart) : null;
  }

  async getById(id: string): Promise<CartResponseDto> {
    const cart = await this.cartRepository.findById(id);
    if (!cart) {
      throw new Error('Cart not found');
    }
    return this.toResponseDto(cart);
  }

  async create(dto: CartCreateDto): Promise<CartResponseDto> {
    const totalAmount = dto.items.reduce((sum, item) => sum + (item.price * item.quantity), 0);
    const cart: Cart = {
      userId: dto.userId,
      items: dto.items,
      totalAmount,
      createdAt: new Date(),
      updatedAt: new Date()
    };
    const created = await this.cartRepository.create(cart);
    return this.toResponseDto(created);
  }

  async update(id: string, dto: CartUpdateDto): Promise<CartResponseDto> {
    const existing = await this.cartRepository.findById(id);
    if (!existing) {
      throw new Error('Cart not found');
    }
    const updated = await this.cartRepository.update(id, dto);
    if (!updated) {
      throw new Error('Failed to update cart');
    }
    return this.toResponseDto(updated);
  }

  async delete(id: string): Promise<void> {
    const deleted = await this.cartRepository.delete(id);
    if (!deleted) {
      throw new Error('Cart not found');
    }
  }

  async deleteByUserId(userId: string): Promise<void> {
    await this.cartRepository.deleteByUserId(userId);
  }

  private toResponseDto(cart: Cart): CartResponseDto {
    return {
      _id: cart._id!,
      userId: cart.userId,
      items: cart.items,
      totalAmount: cart.totalAmount,
      ...(cart.createdAt && { createdAt: cart.createdAt }),
      ...(cart.updatedAt && { updatedAt: cart.updatedAt })
    };
  }
}



