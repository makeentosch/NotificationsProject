using FluentResults;
using Mail.Application.Services.Interfaces;
using Mail.Domain.Interfaces;
using Mail.Domain.Models;

namespace Mail.Application.Services;

public class StatusService(IMailNotificationRepository repository) : IStatusService
{
    public async Task<Result<NotificationStatus>> GetStatusByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var status = await repository.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return status is null ? Result.Fail("Status not found") : Result.Ok(status.Status);
    }
}