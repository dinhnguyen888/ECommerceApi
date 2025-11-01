import mongoose, { Schema, Document } from 'mongoose';
import { EmailLog, EmailStatus } from '../../../application/entities/EmailLog';

export interface EmailLogDocument extends Document, Omit<EmailLog, 'id'> {
  _id: mongoose.Types.ObjectId;
}

const EmailLogSchema = new Schema({
  notificationId: { type: String, required: false },
  email: { type: String, required: true },
  sentAt: { type: Date, default: Date.now },
  status: { 
    type: String, 
    enum: Object.values(EmailStatus),
    required: true 
  }
}, { timestamps: true });

export const EmailLogModel = mongoose.model<EmailLogDocument>('EmailLog', EmailLogSchema);