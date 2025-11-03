import { Router } from 'express';
import { EmailController } from '../controllers/EmailController';
import { EmailService } from '../../../application/services/EmailService';
import { EmailLogRepository } from '../../../infrastructure/repositories/EmailLogRepository';
import { NodemailerEmailService } from '../../../infrastructure/config/email';
import { authenticate, authenticateAndAuthorize } from '../middlewares/authMiddleware';

const emailLogRepository = new EmailLogRepository();
const emailServiceImpl = new NodemailerEmailService();
const emailService = new EmailService(emailServiceImpl, emailLogRepository);
const emailController = new EmailController(emailService);

const router = Router();

// Route mac dinh cho /emails - cong khai
router.get('/', (req, res) => {
  res.status(200).json({ message: 'Email API is working' });
});

// Gui email - yeu cau role Admin
router.post('/send', ...authenticateAndAuthorize('Admin'), emailController.sendEmail);

// Lay log email - yeu cau role Admin
router.get('/logs/:notificationId', ...authenticateAndAuthorize('Admin'), emailController.getEmailLogsByNotificationId);

export default router;