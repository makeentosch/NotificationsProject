using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using Push.Application.Services;
using Push.Domain.Interfaces;
using Push.Domain.Models;

namespace Push.Test;

public class StatusServiceTests
{
    private readonly Mock<IPushNotificationRepository> _repositoryMock;
    private readonly StatusService _service;

    public StatusServiceTests()
    {
        _repositoryMock = new Mock<IPushNotificationRepository>();
        _service = new StatusService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetStatusByIdAsync_ShouldReturnStatus_WhenNotificationExists()
    {
        var notificationId = Guid.NewGuid();
        var notification = new PushNotificationModel(notificationId, DateTime.UtcNow);
        notification.SetStatus(NotificationStatus.Send);

        _repositoryMock.Setup(r => r.FirstOrDefaultAsync(
                x => x.Id == notificationId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(notification);

        var cancellationToken = CancellationToken.None;

        
        var result = await _service.GetStatusByIdAsync(notificationId, cancellationToken);

        
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(NotificationStatus.Send);
        _repositoryMock.Verify(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<PushNotificationModel, bool>>>(),
            cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetStatusByIdAsync_ShouldReturnFail_WhenNotificationDoesNotExist()
    {
        var notificationId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.FirstOrDefaultAsync(
                x => x.Id == notificationId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((PushNotificationModel?)null);

        var cancellationToken = CancellationToken.None;

        
        var result = await _service.GetStatusByIdAsync(notificationId, cancellationToken);

        
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message == "Status not found");
        _repositoryMock.Verify(r => r.FirstOrDefaultAsync(
            It.IsAny<Expression<Func<PushNotificationModel, bool>>>(),
            cancellationToken), Times.Once);
    }
}