using Microsoft.Extensions.Configuration;
using Serilog;
using Sms.Application.Services.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Sms.Infrastructure.NotificationSender;

public class SmsNotificationSender : ISmsNotificationSender
{
    private readonly IConfiguration _configuration;
    
    public SmsNotificationSender(IConfiguration configuration)
    {
        _configuration = configuration;
        
        var accountSid = _configuration["Twilio:accountSid"];
        var authToken = _configuration["Twilio:authToken"];
        TwilioClient.Init(accountSid, authToken);
    }
    
    public Task SendAsync(Guid senderId, Guid recipientId, string phoneNumber, string body)
    {
        var messageResource = MessageResource.Create(
            to: new PhoneNumber(phoneNumber),
            from: new PhoneNumber(_configuration["Twilio:phoneNumber"]),
            body: body
        );
        
        Log.Information($"SMS отправлено! SID: {messageResource.Sid}");
        return Task.CompletedTask;
    }
}