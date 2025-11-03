import { Router } from 'express';
import { NotificationController } from '../controllers/NotificationController';
import { NotificationService } from '../../../application/services/NotificationService';
import { NotificationRepository } from '../../../infrastructure/repositories/NotificationRepository';
import { authenticate, authenticateAndAuthorize } from '../middlewares/authMiddleware';

const notificationRepository = new NotificationRepository();
const notificationService = new NotificationService(notificationRepository);
const notificationController = new NotificationController(notificationService);

const router = Router();

// GET tat ca - yeu cau role Admin (dat truoc de tranh conflict voi /:id)
router.get('/', ...authenticateAndAuthorize('Admin'), notificationController.getAllNotifications);

// POST - yeu cau xac thuc (Client hoac Admin)
router.post('/', authenticate, notificationController.createNotification);

// GET theo userId - yeu cau xac thuc, nguoi dung chi co the xem thong bao cua chinh ho (tru khi la Admin)
router.get('/user/:userId', authenticate, notificationController.getNotificationsByUserId);

// GET theo ID - yeu cau xac thuc (dat sau /user/:userId de tranh conflict)
router.get('/:id', authenticate, notificationController.getNotificationById);

export default router;