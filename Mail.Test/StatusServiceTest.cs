using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using Mail.Application.Services;
using Mail.Domain.Interfaces;
using Mail.Domain.Models;

namespace Mail.Test;

public class StatusServiceTest
{
    private readonly Mock<IMailNotificationRepository> _repositoryMock;
    private readonly StatusService _service;

    public StatusServiceTest()
    {
        _repositoryMock = new Mock<IMailNotificationRepository>();
        _service = new StatusService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetStatusByIdAsync_ShouldReturnStatus_WhenNotificationExists()
    {
        // Arrange
        var notificationId = Guid.NewGuid();
        var notification = new MailNotificationModel(notificationId, DateTime.UtcNow);
        notification.SetStatus(NotificationStatus.Send);

        _repositoryMock.Setup(r => r.FirstOrDefaultAsync(
                x => x.Id == notificationId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(notification);

        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _service.GetStatusByIdAsync(notificationId, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(NotificationStatus.Send);
        _repositoryMock.Verify(r => r.FirstOrDefaultAsync(
            It.IsAny<Expression<Func<MailNotificationModel, bool>>>(),
            cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetStatusByIdAsync_ShouldReturnFail_WhenNotificationDoesNotExist()
    {
        // Arrange
        var notificationId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.FirstOrDefaultAsync(
                x => x.Id == notificationId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((MailNotificationModel?)null);

        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _service.GetStatusByIdAsync(notificationId, cancellationToken);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message == "Status not found");
        _repositoryMock.Verify(r => r.FirstOrDefaultAsync(
            It.IsAny<Expression<Func<MailNotificationModel, bool>>>(),
            cancellationToken), Times.Once);
    }
}