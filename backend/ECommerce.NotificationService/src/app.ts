import express from 'express';
import cors from 'cors';
import helmet from 'helmet';
import morgan from 'morgan';
import * as dotenv from 'dotenv';
import { connectDatabase } from './infrastructure/config/database';
import { RabbitMqConnection } from './infrastructure/messageBroker/rabbitmqConnection';
import { NotificationRepository } from './infrastructure/repositories/NotificationRepository';
import { EmailLogRepository } from './infrastructure/repositories/EmailLogRepository';
import { NotificationService } from './application/services/NotificationService';
import { EmailService } from './application/services/EmailService';
import { NodemailerEmailService } from './infrastructure/config/email';
import { setupMessageConsumers } from './presentation/background/messageConsumers';
import routes from './presentation/http/routes';
import { errorHandler } from './presentation/http/middlewares/errorHandler';

dotenv.config();

const app = express();

connectDatabase().then(async () => {
    const rabbitMqConnection = new RabbitMqConnection();
    await rabbitMqConnection.connect();

    const notificationRepository = new NotificationRepository();
    const notificationService = new NotificationService(notificationRepository);

    const emailLogRepository = new EmailLogRepository();
    const nodemailerEmailService = new NodemailerEmailService();
    const emailService = new EmailService(nodemailerEmailService, emailLogRepository);

    await setupMessageConsumers(rabbitMqConnection, notificationService, emailService);
    console.log('RabbitMQ consumers da duoc khoi tao');
}).catch(err => {
    console.error('Loi khoi tao RabbitMQ:', err);
});

app.use(cors());
app.use(helmet());
app.use(morgan('dev'));
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

app.use('/api', routes);

app.use(errorHandler);

export default app;

