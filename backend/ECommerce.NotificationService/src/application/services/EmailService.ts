import { IEmailService, EmailOptions } from '../interfaces/IEmailService';
import { IEmailLogRepository } from '../interfaces/IEmailLogRepository';
import { EmailLog, EmailStatus } from '../entities/EmailLog';
import { SendEmailDto } from '../dtos/EmailDto';

export class EmailService {
  constructor(
    private emailService: IEmailService,
    private emailLogRepository: IEmailLogRepository
  ) {}

  async sendEmail(dto: SendEmailDto): Promise<boolean> {
    const { to, subject, text, html, notificationId } = dto;
    
    const options: EmailOptions = {
      to,
      subject,
      text,
      html
    };

    try {
      const result = await this.emailService.sendEmail(options);
      
      // Log the email
      const emailLog: EmailLog = {
        notificationId,
        email: Array.isArray(to) ? to.join(', ') : to,
        sentAt: new Date(),
        status: result ? EmailStatus.SUCCESS : EmailStatus.FAILED
      };
      
      await this.emailLogRepository.create(emailLog);
      
      return result;
    } catch (error) {
      // Log failed email
      const emailLog: EmailLog = {
        notificationId,
        email: Array.isArray(to) ? to.join(', ') : to,
        sentAt: new Date(),
        status: EmailStatus.FAILED
      };
      
      await this.emailLogRepository.create(emailLog);
      
      return false;
    }
  }

  async getEmailLogsByNotificationId(notificationId: string): Promise<EmailLog[]> {
    return this.emailLogRepository.findByNotificationId(notificationId);
  }
}