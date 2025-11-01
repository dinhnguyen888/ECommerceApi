import { NotificationType } from '../entities/Notification';

export interface CreateNotificationDto {
  userId?: string;
  title: string;
  message: string;
  type: NotificationType;
}

export interface NotificationResponseDto {
  id: string;
  userId?: string;
  title: string;
  message: string;
  sentAt: Date;
  type: NotificationType;
}