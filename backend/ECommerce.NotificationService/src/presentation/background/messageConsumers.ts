import { RabbitMqConnection } from '../../infrastructure/messageBroker/rabbitmqConnection';
import { RabbitMqConsumer } from '../../infrastructure/messageBroker/rabbitmqConsumer';
import { NotificationService } from '../../application/services/NotificationService';
import { EmailService } from '../../application/services/EmailService';
import { NotificationType } from '../../application/entities/Notification';

interface UserRegisteredEvent {
    UserId: string;
    Email: string;
    UserName: string;
    RegisteredAt: string;
    VerifyUrl: string; 
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
    notificationService: NotificationService,
    emailService: EmailService
): Promise<void> {
    const consumer = new RabbitMqConsumer(rabbitMqConnection);

    await consumer.consume<UserRegisteredEvent>('user.registered', async (event) => {
        // Tao notification
        const notification = await notificationService.createNotification({
            userId: event.UserId,
            title: 'Chao mung ban den voi he thong!',
            message: `Xin chao ${event.UserName}, cam on ban da dang ky tai khoan. Vui long xac thuc email cua ban.`,
            type: NotificationType.SYSTEM
        });

        // Gui email voi verify URL
        const emailSubject = 'Xac thuc dang ky tai khoan';
        const emailHtml = `
            <html>
                <body>
                    <h2>Xin chào ${event.UserName}!</h2>
                    <p>Cảm ơn bạn đã đăng ký tài khoản tại hệ thống E-Commerce.</p>
                    <p>Vui lòng click vào link bên dưới để xác thực email của bạn:</p>
                    <p><a href="${event.VerifyUrl}" style="background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;">Xác thực Email</a></p>
                    <p>Hoặc copy link sau vào trình duyệt:</p>
                    <p>${event.VerifyUrl}</p>
                    <p><strong>Lưu ý:</strong> Link này sẽ hết hạn sau 5 phút.</p>
                    <p>Trân trọng,<br>Đội ngũ E-Commerce</p>
                </body>
            </html>
        `;
        const emailText = `
            Xin chào ${event.UserName}!
            
            Cảm ơn bạn đã đăng ký tài khoản tại hệ thống E-Commerce.
            Vui lòng click vào link sau để xác thực email của bạn:
            ${event.VerifyUrl}
            
            Lưu ý: Link này sẽ hết hạn sau 5 phút.
            
            Trân trọng,
            Đội ngũ E-Commerce
        `;

        await emailService.sendEmail({
            to: event.Email,
            subject: emailSubject,
            text: emailText,
            html: emailHtml,
            notificationId: notification.id
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
