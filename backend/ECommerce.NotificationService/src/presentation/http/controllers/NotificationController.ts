import { Request, Response } from 'express';
import { NotificationService } from '../../../application/services/NotificationService';
import { CreateNotificationDto } from '../../../application/dtos/NotificationDto';

export class NotificationController {
  constructor(private notificationService: NotificationService) {}

  async createNotification(req: Request, res: Response): Promise<void> {
    try {
      const notificationDto: CreateNotificationDto = req.body;
      const notification = await this.notificationService.createNotification(notificationDto);
      res.status(201).json(notification);
    } catch (error) {
      console.error('Error creating notification:', error);
      res.status(500).json({ message: 'Internal server error' });
    }
  }

  async getNotificationById(req: Request, res: Response): Promise<void> {
    try {
      const { id } = req.params;
      if (!id) {
        res.status(400).json({ message: 'Notification ID is required' });
        return;
      }
      const notification = await this.notificationService.getNotificationById(id);
      
      if (!notification) {
        res.status(404).json({ message: 'Notification not found' });
        return;
      }
      
      res.status(200).json(notification);
    } catch (error) {
      console.error('Error getting notification:', error);
      res.status(500).json({ message: 'Internal server error' });
    }
  }

  async getNotificationsByUserId(req: Request, res: Response): Promise<void> {
    try {
      const { userId } = req.params;
      if (!userId) {
        res.status(400).json({ message: 'User ID is required' });
        return;
      }
      const notifications = await this.notificationService.getNotificationsByUserId(userId);
      res.status(200).json(notifications);
    } catch (error) {
      console.error('Error getting notifications by user ID:', error);
      res.status(500).json({ message: 'Internal server error' });
    }
  }

  async getAllNotifications(req: Request, res: Response): Promise<void> {
    try {
      const notifications = await this.notificationService.getAllNotifications();
      res.status(200).json(notifications);
    } catch (error) {
      console.error('Error getting all notifications:', error);
      res.status(500).json({ message: 'Internal server error' });
    }
  }
}