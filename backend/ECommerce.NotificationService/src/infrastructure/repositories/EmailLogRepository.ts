import { IEmailLogRepository } from '../../application/interfaces/IEmailLogRepository';
import { EmailLog } from '../../application/entities/EmailLog';
import { EmailLogModel } from '../persistence/models/EmailLogModel';

export class EmailLogRepository implements IEmailLogRepository {
  async create(emailLog: EmailLog): Promise<EmailLog> {
    const newEmailLog = await EmailLogModel.create(emailLog);
    return this.mapToEntity(newEmailLog);
  }

  async findById(id: string): Promise<EmailLog | null> {
    const emailLog = await EmailLogModel.findById(id);
    return emailLog ? this.mapToEntity(emailLog) : null;
  }

  async findByNotificationId(notificationId: string): Promise<EmailLog[]> {
    const emailLogs = await EmailLogModel.find({ notificationId });
    return emailLogs.map(log => this.mapToEntity(log));
  }

  async findByEmail(email: string): Promise<EmailLog[]> {
    const emailLogs = await EmailLogModel.find({ email });
    return emailLogs.map(log => this.mapToEntity(log));
  }

  private mapToEntity(document: any): EmailLog {
    return {
      id: document._id.toString(),
      notificationId: document.notificationId,
      email: document.email,
      sentAt: document.sentAt,
      status: document.status
    };
  }
}