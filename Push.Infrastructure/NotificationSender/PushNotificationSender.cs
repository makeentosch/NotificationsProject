using FirebaseAdmin.Messaging;
using Push.Application.Services.Interfaces;
using Serilog;

namespace Push.Infrastructure.NotificationSender;

public class PushNotificationSender : IPushNotificationSender
{
    public async Task SendAsync(string deviceToken, string title, string body)
    {
        var message = new Message
        {
            Notification = new Notification
            {
                Title = title,
                Body = body,
            },
            Token = deviceToken, // Replace with the actual device token of the target device
        };
        
        var messaging = FirebaseMessaging.DefaultInstance;
        var result = await messaging.SendAsync(message);

        if (string.IsNullOrEmpty(result))
            Log.Warning("Error sending the message");
        else
            Log.Information("Message sent successfully!");
    }
}