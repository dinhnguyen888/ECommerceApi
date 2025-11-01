import { Router } from 'express';
import { NotificationController } from '../controllers/NotificationController';
import { NotificationService } from '../../../application/services/NotificationService';
import { NotificationRepository } from '../../../infrastructure/repositories/NotificationRepository';

const notificationRepository = new NotificationRepository();
const notificationService = new NotificationService(notificationRepository);
const notificationController = new NotificationController(notificationService);

const router = Router();

router.post('/', notificationController.createNotification);
router.get('/:id', notificationController.getNotificationById);
router.get('/user/:userId', notificationController.getNotificationsByUserId);
router.get('/', notificationController.getAllNotifications);

export default router;