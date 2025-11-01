import { INotificationRepository } from '../../application/interfaces/INotificationRepository';
import { Notification } from '../../application/entities/Notification';
import { NotificationModel } from '../persistence/models/NotificationModel';

export class NotificationRepository implements INotificationRepository {
  async create(notification: Notification): Promise<Notification> {
    const newNotification = await NotificationModel.create(notification);
    return this.mapToEntity(newNotification);
  }

  async findById(id: string): Promise<Notification | null> {
    const notification = await NotificationModel.findById(id);
    return notification ? this.mapToEntity(notification) : null;
  }

  async findByUserId(userId: string): Promise<Notification[]> {
    const notifications = await NotificationModel.find({ userId });
    return notifications.map(notification => this.mapToEntity(notification));
  }

  async findAll(): Promise<Notification[]> {
    const notifications = await NotificationModel.find();
    return notifications.map(notification => this.mapToEntity(notification));
  }

  async update(id: string, notification: Partial<Notification>): Promise<Notification | null> {
    const updatedNotification = await NotificationModel.findByIdAndUpdate(
      id,
      notification,
      { new: true }
    );
    return updatedNotification ? this.mapToEntity(updatedNotification) : null;
  }

  async delete(id: string): Promise<boolean> {
    const result = await NotificationModel.findByIdAndDelete(id);
    return !!result;
  }

  private mapToEntity(document: any): Notification {
    return {
      id: document._id.toString(),
      userId: document.userId,
      title: document.title,
      message: document.message,
      sentAt: document.sentAt,
      type: document.type
    };
  }
}