using Core.Domain.Constants;
using Core.Domain.Repositories.Interfaces;
using Core.Domain.Repositories.Models;

namespace Notification.Domain.Models;

public class NotificationModel : EntityBase<Guid>, IAggregateRoot
{
    public Guid SenderId { get; private set; }
    
    public Guid RecipientId { get; private set; }
    
    public string? RecipientMail { get; private set; }
    
    public string? RecipientPhoneNumber { get; private set; }
    
    public string? RecipientDeviceToken { get; private set; }

    public List<NotificationType> NotificationTypes { get; private set; }
    
    public string Content { get; private set; }

    public List<Attachment> Attachments { get; private set; }
    
    public NotificationModel(
        Guid recipientId,
        List<NotificationType> notificationTypes,
        string content,
        DateTime dateOfCreate) 
        : this(DataConstants.AdminUserId, recipientId, notificationTypes, content, dateOfCreate)
    {
    }

    public NotificationModel(
        Guid senderId,
        Guid recipientId,
        List<NotificationType> notificationTypes,
        string content,
        DateTime dateOfCreate)
        : this(
            senderId,
            recipientId,
            null,
            null,
            null,
            notificationTypes,
            content,
            [],
            dateOfCreate)
    {
    }

    public NotificationModel(
        Guid senderId,
        Guid recipientId,
        string? recipientMail,
        string? recipientPhoneNumber,
        string? recipientDeviceToken,
        List<NotificationType> notificationTypes,
        string content,
        List<Attachment> attachments,
        DateTime dateOfCreate)
        : base(Guid.NewGuid(), dateOfCreate)
    {
        SenderId = senderId;
        RecipientId = recipientId;
        RecipientMail = recipientMail;
        RecipientPhoneNumber = recipientPhoneNumber;
        RecipientDeviceToken = recipientDeviceToken;
        NotificationTypes = notificationTypes;
        Content = content;
        Attachments = attachments;
    }

    public void AddAttachment(Attachment attachment)
    {
        Attachments.Add(attachment);
    }
}
