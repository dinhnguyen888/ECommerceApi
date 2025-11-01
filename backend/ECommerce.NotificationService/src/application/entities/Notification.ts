export enum NotificationType {
  EMAIL = 'email',
  SYSTEM = 'system',
  PERSONAL = 'personal'
}

export interface Notification {
  id?: string;
  userId?: string;
  title: string;
  message: string;
  sentAt: Date;
  type: NotificationType;
}