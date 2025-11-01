import { Notification, NotificationType } from '../entities/Notification';
import { INotificationRepository } from '../interfaces/INotificationRepository';
import { CreateNotificationDto } from '../dtos/NotificationDto';

export class NotificationService {
  constructor(private notificationRepository: INotificationRepository) {}

  async createNotification(dto: CreateNotificationDto): Promise<Notification> {
    const notification: Notification = {
      ...dto,
      sentAt: new Date()
    };

    return this.notificationRepository.create(notification);
  }

  async getNotificationById(id: string): Promise<Notification | null> {
    return this.notificationRepository.findById(id);
  }

  async getNotificationsByUserId(userId: string): Promise<Notification[]> {
    return this.notificationRepository.findByUserId(userId);
  }

  async getAllNotifications(): Promise<Notification[]> {
    return this.notificationRepository.findAll();
  }
}