export enum EmailStatus {
  SUCCESS = 'success',
  FAILED = 'failed'
}

export interface EmailLog {
  id?: string;
  notificationId?: string;
  email: string;
  sentAt: Date;
  status: EmailStatus;
}