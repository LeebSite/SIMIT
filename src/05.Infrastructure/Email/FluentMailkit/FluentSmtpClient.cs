using MailKit;
using MailKit.Net.Smtp;
using MimeKit;

namespace Pertamina.SIMIT.Infrastructure.Email.FluentMailkit;

public class FluentSmtpClient : SmtpClient
{
    public FluentSmtpClient() : base()
    {
    }

    protected override string GetEnvelopeId(MimeMessage message)
    {
        // Since you will want to be able to map whatever identifier you return here to the
        // message, the obvious identifier to use is probably the Message-Id value.
        return message.MessageId;
    }

    protected override DeliveryStatusNotification? GetDeliveryStatusNotifications(MimeMessage message, MailboxAddress mailbox)
    {
        return DeliveryStatusNotification.Failure | DeliveryStatusNotification.Delay | DeliveryStatusNotification.Success;
    }
}
