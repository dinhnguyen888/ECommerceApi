import { EmailLog } from '../entities/EmailLog';

export interface IEmailLogRepository {
  create(emailLog: EmailLog): Promise<EmailLog>;
  findById(id: string): Promise<EmailLog | null>;
  findByNotificationId(notificationId: string): Promise<EmailLog[]>;
  findByEmail(email: string): Promise<EmailLog[]>;
}