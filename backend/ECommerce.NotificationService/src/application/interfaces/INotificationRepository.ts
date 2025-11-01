import { Notification } from '../entities/Notification';

export interface INotificationRepository {
  create(notification: Notification): Promise<Notification>;
  findById(id: string): Promise<Notification | null>;
  findByUserId(userId: string): Promise<Notification[]>;
  findAll(): Promise<Notification[]>;
  update(id: string, notification: Partial<Notification>): Promise<Notification | null>;
  delete(id: string): Promise<boolean>;
}