using Notification.Api.Controllers.Notifications.Dtos;

namespace Notification.Api.Controllers.Notifications.Responses;

public record NotificationResponse
{
    public Guid Id { get; init; }
    
    public Guid? SenderId { get; init; }
    
    public Guid RecipientId { get; init; }
    
    public List<NotificationTypeDto>? NotificationTypes { get; init; }
    
    public required string Content { get; init; }
    
    public IEnumerable<AttachmentDto> Attachments { get; init; }
    
    public DateTime DateOfCreate { get; init; }
    
    public DateTime? DateOfUpdate { get; init; }
}