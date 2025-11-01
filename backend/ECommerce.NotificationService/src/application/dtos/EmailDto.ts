import { EmailStatus } from '../entities/EmailLog';

export interface SendEmailDto {
  to: string | string[];
  subject: string;
  text?: string;
  html?: string;
  notificationId?: string;
}

export interface EmailLogResponseDto {
  id: string;
  notificationId?: string;
  email: string;
  sentAt: Date;
  status: EmailStatus;
}