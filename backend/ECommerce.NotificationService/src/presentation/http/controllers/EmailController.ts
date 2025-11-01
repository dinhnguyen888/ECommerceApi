import { Request, Response } from 'express';
import { EmailService } from '../../../application/services/EmailService';
import { SendEmailDto } from '../../../application/dtos/EmailDto';

export class EmailController {
  constructor(private emailService: EmailService) {}

  async sendEmail(req: Request, res: Response): Promise<void> {
    try {
      const emailDto: SendEmailDto = req.body;
      const result = await this.emailService.sendEmail(emailDto);
      
      if (result) {
        res.status(200).json({ success: true, message: 'Email sent successfully' });
      } else {
        res.status(400).json({ success: false, message: 'Failed to send email' });
      }
    } catch (error) {
      console.error('Error sending email:', error);
      res.status(500).json({ message: 'Internal server error' });
    }
  }

  async getEmailLogsByNotificationId(req: Request, res: Response): Promise<void> {
    try {
      const { notificationId } = req.params;
      if (!notificationId) {
        res.status(400).json({ message: 'Notification ID is required' });
        return;
      }
      const emailLogs = await this.emailService.getEmailLogsByNotificationId(notificationId);
      res.status(200).json(emailLogs);
    } catch (error) {
      console.error('Error getting email logs:', error);
      res.status(500).json({ message: 'Internal server error' });
    }
  }
}