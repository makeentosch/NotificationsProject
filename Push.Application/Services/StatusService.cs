using FluentResults;
using Push.Application.Services.Interfaces;
using Push.Domain.Interfaces;
using Push.Domain.Models;

namespace Push.Application.Services;

public class StatusService(IPushNotificationRepository repository) : IStatusService
{
    public async Task<Result<NotificationStatus>> GetStatusByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var status = await repository.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return status is null ? Result.Fail("Status not found") : Result.Ok(status.Status);
    }
}