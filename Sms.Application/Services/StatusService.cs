using FluentResults;
using Sms.Application.Services.Interfaces;
using Sms.Domain.Interfaces;
using Sms.Domain.Models;

namespace Sms.Application.Services;

public class StatusService(ISmsNotificationRepository repository) : IStatusService
{
    public async Task<Result<NotificationStatus>> GetStatusByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var status = await repository.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return status is null ? Result.Fail("Status not found") : Result.Ok(status.Status);
    }
}