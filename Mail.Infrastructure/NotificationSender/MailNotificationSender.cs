using System.Net.Mail;
using Mail.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using MimeKit;
using RabbitMqContracts.Mail.Dtos;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Mail.Infrastructure.NotificationSender;

public class MailNotificationSender(IConfiguration configuration) : IMailNotificationSender
{
    public async Task SendAsync(Guid senderId, Guid recipientId, MailAddress recipientMail, string body, IEnumerable<AttachmentDto> attachments)
    {
        var mailName = configuration["Mail:Name"];
        var mailAddress = configuration["Mail:Address"];
        
        var bodyBuilder = new BodyBuilder
        {
            TextBody = body
        };

            foreach (var attachment in attachments)
            {
                using var webClient = new HttpClient();
                var fileData = await webClient.GetByteArrayAsync(attachment.FileUri);

                bodyBuilder.Attachments.Add(attachment.FileName, fileData, ContentType.Parse(attachment.ContentType));
            }

        var message = new MimeMessage
        {
            From = { new MailboxAddress(mailName, mailAddress) },
            To = { new MailboxAddress(recipientId.ToString(), recipientMail.Address) },
            Subject = "Notification",
            Body = bodyBuilder.ToMessageBody()
        };
        //
        // var message = new MimeMessage
        // {
        //     From = { new MailboxAddress(mailName, mailAddress) },
        //     To = { new MailboxAddress(recipientId.ToString(), recipientMail.Address) },
        //     Subject = "Notification",
        //     Body = new TextPart("plain")
        //     {
        //         Text = body,
        //     },
        // };

        using var client = new SmtpClient();
        await client.ConnectAsync(
            configuration["Mail:Smtp:Host"],
            int.Parse(configuration["Mail:Smtp:Port"] ?? "25"), false);

        await client.AuthenticateAsync(
            configuration["Mail:Smtp:Login"],
            configuration["Mail:Smtp:Password"]);
            
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}