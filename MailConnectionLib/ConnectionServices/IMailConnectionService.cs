using RabbitMqContracts.Mail.Requests;
using RabbitMqContracts.Mail.Responses;

namespace MailConnectionLib.ConnectionServices;

public interface IMailConnectionService
{
    Task SendNotificationAsync(SendMailNotificationRequest request);
    
    Task<GetMailStatusResponse> GetMailStatusAsync(GetMailStatusRequest request);
}