using Mapster;
using Notification.Api.Controllers.Notifications.Dtos;
using Notification.Api.Controllers.Notifications.Requests;
using Notification.Api.Controllers.Notifications.Responses;
using Notification.Domain.Models;

namespace Notification.Api.Mappers;

internal static class NotificationMapper
{
    public static NotificationResponse ToNotificationResponse(this NotificationModel model)
    {
        return new NotificationResponse
        {
            Id = model.Id,
            SenderId = model.SenderId,
            RecipientId = model.RecipientId,
            Content = model.Content,
            Attachments = model.Attachments.Adapt<IEnumerable<AttachmentDto>>(),
            DateOfCreate = model.DateOfCreate,
        };
    }
    
    public static NotificationWithStatusResponse ToNotificationWithStatusResponse(this NotificationModel model, Dictionary<int, int> status)
    {
        return new NotificationWithStatusResponse
        {
            Id = model.Id,
            SenderId = model.SenderId,
            RecipientId = model.RecipientId,
            Statuses = model.NotificationTypes.Select(x => x.ToNotificationStatus(status)).ToList(),
            Attachments = model.Attachments.Adapt<IEnumerable<AttachmentDto>>(),
            Content = model.Content,
            DateOfCreate = model.DateOfCreate,
        };
    }

    private static NotificationStatus ToNotificationStatus(this NotificationType type, Dictionary<int, int> status)
    {
        if (!status.ContainsKey((int)type))
            return new NotificationStatus((int) type, 0);
        
        return new NotificationStatus((int) type, status[(int) type]);
    }
    
    public static IEnumerable<NotificationResponse> ToNotificationResponse(this IEnumerable<NotificationModel> models)
    {
        return models.Select(x => x.ToNotificationResponse());
    }

    public static NotificationModel ToNotificationModel(this NotificationRequest request, Guid senderId, DateTime dateOfCreate)
    {
        return new NotificationModel(
            senderId,
            request.RecipientId,
            request.RecipientMail,
            request.RecipientPhoneNumber,
            request.RecipientDeviceToken,
            request.NotificationTypes.ToNotificationType(),
            request.Content,
            request.Attachments.Adapt<List<Attachment>>(),
            dateOfCreate);
    }

    public static NotificationType ToNotificationType(this NotificationTypeDto notificationTypeDto)
    {
        return Enum.Parse<NotificationType>(notificationTypeDto.ToString());
    }
    
    public static List<NotificationType> ToNotificationType(this IEnumerable<NotificationTypeDto> notificationTypeDtos)
    {
        return notificationTypeDtos.Select(x => x.ToNotificationType()).ToList();
    }
}