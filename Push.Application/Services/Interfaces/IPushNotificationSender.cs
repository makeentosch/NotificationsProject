namespace Push.Application.Services.Interfaces;

public interface IPushNotificationSender
{
    Task SendAsync(string deviceToken, string title, string body);
}
