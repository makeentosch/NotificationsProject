namespace Mail.Domain.Models;

public enum NotificationStatus
{
    Created = 1,
    Retry = 2,
    Send = 4,
    Fail = 8,
}