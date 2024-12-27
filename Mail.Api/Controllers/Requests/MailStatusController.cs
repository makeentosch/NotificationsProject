using Mail.Domain.Models;

namespace Mail.Api.Controllers.Requests;

public record MailStatusController
{
    public Guid Id { get; set; }
    
    public NotificationStatus Status { get; set; }
}