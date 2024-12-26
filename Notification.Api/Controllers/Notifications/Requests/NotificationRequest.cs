using System.ComponentModel.DataAnnotations;
using Notification.Api.Controllers.Notifications.Dtos;

namespace Notification.Api.Controllers.Notifications.Requests;

public record NotificationRequest
{
    [Required]
    public required Guid RecipientId { get; init; }
    
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Не валидная почта.")]
    public string? RecipientMail { get; init; }
    
    [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Не валидный номер телефона.")]
    public string? RecipientPhoneNumber { get; init; }
    
    public string? RecipientDeviceToken { get; init; }
    
    [Required]
    public required List<NotificationTypeDto> NotificationTypes { get; init; }
    
    [Required]
    public required string Content { get; init; }
    
    public IEnumerable<AttachmentDto> Attachments { get; init; }
}