using Core.Domain.Repositories.Interfaces;
using Core.Domain.Repositories.Models;

namespace Sms.Domain.Models;

public class SmsNotificationModel : EntityBase<Guid>, IAggregateRoot
{
    public NotificationStatus Status { get; private set; }

    public SmsNotificationModel(Guid id, DateTime dateOfCreate) : base(id, dateOfCreate)
    {
    }

    public void SetStatus(NotificationStatus status)
    {
        if (Status == NotificationStatus.Send && status == NotificationStatus.Fail)
            throw new InvalidOperationException("Cannot set status to Send or Fail");   
        
        Status = status;
        SetDateOfUpdate(DateTime.UtcNow);
    }

    public void SetDateOfUpdate(DateTime dateOfUpdate)
    {
        DateOfUpdate = dateOfUpdate;
    }
}