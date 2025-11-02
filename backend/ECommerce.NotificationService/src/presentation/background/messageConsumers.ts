import { RabbitMqConnection } from '../../infrastructure/messageBroker/rabbitmqConnection';
import { RabbitMqConsumer } from '../../infrastructure/messageBroker/rabbitmqConsumer';
import { NotificationService } from '../../application/services/NotificationService';
import { NotificationType } from '../../application/entities/Notification';

interface UserRegisteredEvent {
    UserId: string;
    Email: string;
    UserName: string;
    RegisteredAt: string;
}

interface OrderCreatedEvent {
    OrderId: string;
    UserId: string;
    TotalAmount: number;
    CreatedAt: string;
}

interface PaymentCompletedEvent {
    PaymentId: string;
    OrderId: string;
    UserId: string;
    Amount: number;
    PaidAt: string;
}

export async function setupMessageConsumers(
    rabbitMqConnection: RabbitMqConnection,
    notificationService: NotificationService
): Promise<void> {
    const consumer = new RabbitMqConsumer(rabbitMqConnection);

    await consumer.consume<UserRegisteredEvent>('user.registered', async (event) => {
        await notificationService.createNotification({
            userId: event.UserId,
            title: 'Chao mung ban den voi he thong!',
            message: `Xin chao ${event.UserName}, cam on ban da dang ky tai khoan.`,
            type: NotificationType.SYSTEM
        });
    });

    await consumer.consume<OrderCreatedEvent>('order.created', async (event) => {
        await notificationService.createNotification({
            userId: event.UserId,
            title: 'Don hang da duoc tao',
            message: `Don hang ${event.OrderId} cua ban da duoc tao thanh cong voi tong tien ${event.TotalAmount} VND.`,
            type: NotificationType.SYSTEM
        });
    });

    await consumer.consume<PaymentCompletedEvent>('payment.completed', async (event) => {
        await notificationService.createNotification({
            userId: event.UserId,
            title: 'Thanh toan thanh cong',
            message: `Don hang ${event.OrderId} cua ban da duoc thanh toan thanh cong voi so tien ${event.Amount} VND.`,
            type: NotificationType.SYSTEM
        });
    });
}
