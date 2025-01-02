using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using Notification.Application;
using Notification.Domain.Interfaces;
using Notification.Domain.Models;
using Notification.Domain.Models.Enums;
using MailConnectionLib.ConnectionServices;
using PushConnectionLib.ConnectionServices;
using SmsConnectionLib.ConnectionServices;
using RabbitMqContracts.Mail.Requests;
using RabbitMqContracts.Mail.Responses;
using RabbitMqContracts.Sms.Requests;

namespace Notification.Test;

public class NotificationServiceTest
{
    private readonly Mock<INotificationRepository> _repositoryMock;
    private readonly Mock<IMailConnectionService> _mailServiceMock;
    private readonly Mock<ISmsConnectionService> _smsServiceMock;
    private readonly Mock<IPushConnectionService> _pushServiceMock;
    private readonly NotificationService _service;

    public NotificationServiceTest()
    {
        _repositoryMock = new Mock<INotificationRepository>();
        _mailServiceMock = new Mock<IMailConnectionService>();
        _smsServiceMock = new Mock<ISmsConnectionService>();
        _pushServiceMock = new Mock<IPushConnectionService>();
        _service = new NotificationService(
            _repositoryMock.Object,
            _mailServiceMock.Object,
            _smsServiceMock.Object,
            _pushServiceMock.Object
        );
    }

    [Fact]
    public async Task SendNotificationAsync_ShouldSendNotificationsAndSaveToRepository()
    {
        // Arrange
        var model = new NotificationModel(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "mail@mail.com",
            "+79123212112",
            "device-token",
            new List<NotificationType> { NotificationType.Email, NotificationType.Sms },
            "test content",
            [],
            DateTime.UtcNow);

        var cancellationToken = CancellationToken.None;

        _mailServiceMock.Setup(m => m.SendNotificationAsync(It.IsAny<SendMailNotificationRequest>())).Returns(Task.CompletedTask);
        _smsServiceMock.Setup(m => m.SendNotificationAsync(It.IsAny<SendSmsNotificationRequest>())).Returns(Task.CompletedTask);
        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<NotificationModel>(), cancellationToken)).Returns(() => new ValueTask<NotificationModel>());
        _repositoryMock.Setup(r => r.UnitOfWork.SaveChangesAsync(cancellationToken)).ReturnsAsync(1);

        // Act
        await _service.SendNotificationAsync(model, cancellationToken);

        // Assert
        _mailServiceMock.Verify(m => m.SendNotificationAsync(It.IsAny<SendMailNotificationRequest>()), Times.Once);
        _smsServiceMock.Verify(m => m.SendNotificationAsync(It.IsAny<SendSmsNotificationRequest>()), Times.Once);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<NotificationModel>(), cancellationToken), Times.Once);
        _repositoryMock.Verify(r => r.UnitOfWork.SaveChangesAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNotification_WhenItExists()
    {
        // Arrange
        var model = new NotificationModel(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "mail@mail.com",
            "+79123212112",
            "device-token",
            [NotificationType.Email, NotificationType.Sms],
            "test content",
            [],
            DateTime.UtcNow);

        _repositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<NotificationModel, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(model);

        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _service.GetByIdAsync(model.Id, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(model.Id);
        _repositoryMock.Verify(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<NotificationModel, bool>>>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllNotifications()
    {
        // Arrange
        var notifications = new List<NotificationModel>
        {
            new(Guid.NewGuid(), [NotificationType.Email, NotificationType.Sms], "content1", DateTime.UtcNow ),
            new(Guid.NewGuid(), [NotificationType.Email, NotificationType.Sms], "content2", DateTime.UtcNow ),
        };

        _repositoryMock.Setup(r => r.ListAsync(It.IsAny<CancellationToken>())).ReturnsAsync(notifications);

        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _service.GetAllAsync(cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        _repositoryMock.Verify(r => r.ListAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetStatusesByIdAsync_ShouldReturnStatusesForEmail()
    {
        // Arrange
        var notificationId = Guid.NewGuid();
        var types = new List<NotificationType> { NotificationType.Email };

        _mailServiceMock.Setup(m => m.GetMailStatusAsync(It.IsAny<GetMailStatusRequest>()))
            .ReturnsAsync(new GetMailStatusResponse(1));

        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _service.GetStatusesByIdAsync(notificationId, types, cancellationToken);

        // Assert
        result.Should().ContainKey((int)NotificationType.Email);
        result[(int)NotificationType.Email].Should().Be(1);
        _mailServiceMock.Verify(m => m.GetMailStatusAsync(It.IsAny<GetMailStatusRequest>()), Times.Once);
    }
}