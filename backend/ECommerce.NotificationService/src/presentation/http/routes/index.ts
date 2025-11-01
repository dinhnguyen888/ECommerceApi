import { Router } from 'express';
import notificationRoutes from './notificationRoutes';
import emailRoutes from './emailRoutes';

const router = Router();

router.use('/notifications', notificationRoutes);
router.use('/emails', emailRoutes);

export default router;