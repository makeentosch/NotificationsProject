using RabbitMqContracts.Sms.Requests;
using RabbitMqContracts.Sms.Responses;

namespace SmsConnectionLib.ConnectionServices;

public interface ISmsConnectionService
{
    Task SendNotificationAsync(SendSmsNotificationRequest request);
         
    Task<GetSmsStatusResponse> GetSmsStatusAsync(GetSmsStatusRequest request);
}