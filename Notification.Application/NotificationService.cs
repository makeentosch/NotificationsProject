using Notification.Domain.Interfaces;
using Notification.Domain.Models;
using MailConnectionLib.ConnectionServices;
using Mapster;
using Notification.Application.Interfaces;
using PushConnectionLib.ConnectionServices;
using RabbitMqContracts.Mail.Dtos;
using RabbitMqContracts.Mail.Requests;
using RabbitMqContracts.Push.Requests;
using RabbitMqContracts.Sms.Requests;
using SmsConnectionLib.ConnectionServices;

namespace Notification.Application;

public class NotificationService(
    INotificationRepository notificationRepository,
    IMailConnectionService mailConnectionService,
    ISmsConnectionService smsConnectionService,
    IPushConnectionService pushConnectionService) : INotificationService
{
    public async Task SendNotificationAsync(NotificationModel model, CancellationToken cancellationToken = default)
    {
        if (model.NotificationTypes.Contains(NotificationType.Email))
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(model.RecipientMail);
            var attachments = model.Attachments.Adapt<IEnumerable<AttachmentDto>>();
            await mailConnectionService.SendNotificationAsync(new SendMailNotificationRequest(model.Id, model.Content,
                attachments, model.RecipientId, model.RecipientMail!, model.SenderId));
        }
        if (model.NotificationTypes.Contains(NotificationType.Sms))
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(model.RecipientPhoneNumber);
            await smsConnectionService.SendNotificationAsync(new SendSmsNotificationRequest(model.Id, model.SenderId,
                model.RecipientId, model.RecipientPhoneNumber!, model.Content));
        }
        if (model.NotificationTypes.Contains(NotificationType.Push))
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(model.RecipientDeviceToken);
            await pushConnectionService.SendNotificationAsync(
                new SendPushNotificationRequest(model.Id, "Title", model.RecipientDeviceToken!, model.Content!));
        }
        

        await notificationRepository.AddAsync(model, cancellationToken);
        await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public Task<NotificationModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return notificationRepository.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<IReadOnlyList<NotificationModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return notificationRepository.ListAsync(cancellationToken);
    }

    public async Task<Dictionary<int, int>> GetStatusesByIdAsync(Guid id, List<NotificationType> types, CancellationToken cancellationToken = default)
    {
        var response = new Dictionary<int, int>();
        if (types.Contains(NotificationType.Email))
        {
            var status = await mailConnectionService.GetMailStatusAsync(new GetMailStatusRequest(id));
            response.Add((int) NotificationType.Email, status.Status);
        }
        if (types.Contains(NotificationType.Sms))
        {
            var status = await smsConnectionService.GetSmsStatusAsync(new GetSmsStatusRequest(id));
            response.Add((int) NotificationType.Sms, status.Status);
        }
        if (types.Contains(NotificationType.Push))
        {
            var status = await pushConnectionService.GetPushStatusAsync(new GetPushStatusRequest(id));
            response.Add((int) NotificationType.Push, status.Status);
        }

        return response;
    }
}