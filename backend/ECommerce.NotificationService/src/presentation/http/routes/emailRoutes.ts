import { Router } from 'express';
import { EmailController } from '../controllers/EmailController';
import { EmailService } from '../../../application/services/EmailService';
import { EmailLogRepository } from '../../../infrastructure/repositories/EmailLogRepository';
import { NodemailerEmailService } from '../../../infrastructure/config/email';

const emailLogRepository = new EmailLogRepository();
const emailServiceImpl = new NodemailerEmailService();
const emailService = new EmailService(emailServiceImpl, emailLogRepository);
const emailController = new EmailController(emailService);

const router = Router();

// Route mặc định cho /emails
router.get('/', (req, res) => {
  res.status(200).json({ message: 'Email API is working' });
});

router.post('/send', emailController.sendEmail);
router.get('/logs/:notificationId', emailController.getEmailLogsByNotificationId);

export default router;