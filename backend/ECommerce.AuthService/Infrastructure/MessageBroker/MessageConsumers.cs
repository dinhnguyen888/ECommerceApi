using ECommerce.AuthService.Application.Interfaces;
using ECommerce.AuthService.Application.Events;
using ECommerce.AuthService.Application.Services;

namespace ECommerce.AuthService.Infrastructure.MessageBroker
{
    public class MessageConsumers
    {
        private readonly IMessageConsumer _consumer;
        private readonly EmailSentNotificationService _emailSentNotificationService;

        public MessageConsumers(IMessageConsumer consumer, EmailSentNotificationService emailSentNotificationService)
        {
            _consumer = consumer;
            _emailSentNotificationService = emailSentNotificationService;
        }

        public void SetupConsumers()
        {
            // Lang nghe event email da duoc gui tu NotificationService
            _consumer.Consume<EmailSentEvent>("email.sent", (emailEvent) =>
            {
                // Thong bao cho service biet email da duoc gui
                _emailSentNotificationService.NotifyEmailSent(emailEvent);
                
                Console.WriteLine($"Email da duoc gui cho user {emailEvent.UserId}: {(emailEvent.Success ? "Thanh cong" : "That bai")}");
            });
        }
    }
}

