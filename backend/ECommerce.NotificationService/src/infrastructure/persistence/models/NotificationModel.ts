import mongoose, { Schema, Document } from 'mongoose';
import { Notification, NotificationType } from '../../../application/entities/Notification';

export interface NotificationDocument extends Document, Omit<Notification, 'id'> {
  _id: mongoose.Types.ObjectId;
}

const NotificationSchema = new Schema({
  userId: { type: String, required: false },
  title: { type: String, required: true },
  message: { type: String, required: true },
  sentAt: { type: Date, default: Date.now },
  type: { 
    type: String, 
    enum: Object.values(NotificationType),
    required: true 
  }
}, { timestamps: true });

export const NotificationModel = mongoose.model<NotificationDocument>('Notification', NotificationSchema);