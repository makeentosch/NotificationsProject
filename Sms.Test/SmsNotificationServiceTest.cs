using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using RabbitMqContracts.Sms.Requests;
using Sms.Application.Services;
using Sms.Application.Services.Interfaces;
using Sms.Domain.Interfaces;
using Sms.Domain.Models;

namespace Sms.Test;

public class SmsNotificationServiceTest
{
    private readonly Mock<ISmsNotificationRepository> _repositoryMock;
    private readonly Mock<ISmsNotificationSender> _senderMock;
    private readonly ISmsNotificationService _service;

    private readonly Dictionary<string, string> _inMemoryConfiguration = new()
    {
        { "Polly:DefaultAttempts", "3" },
        { "Polly:DefaultWaitTimeInSeconds", "5" }
    };

    public SmsNotificationServiceTest()
    {
        _repositoryMock = new Mock<ISmsNotificationRepository>();
        _senderMock = new Mock<ISmsNotificationSender>();
        
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(_inMemoryConfiguration!)
            .Build();
        _service = new SmsNotificationService(_repositoryMock.Object, _senderMock.Object, config);
    }

    [Fact]
    public async Task SendMailAsync_ShouldSendNotificationSuccessfully()
    {
        // Arrange
        var request = new SendSmsNotificationRequest(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            "+7912345678",
            "test message"
        );
        var cancellationToken = CancellationToken.None;

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<SmsNotificationModel>(), cancellationToken))
            .Returns(() => new ValueTask<SmsNotificationModel>());

        _senderMock.Setup(s => s.SendAsync(request.SenderId, request.RecipientId, It.IsAny<string>(), request.Body))
            .Returns(() => Task.CompletedTask);

        _repositoryMock.Setup(r => r.UnitOfWork.SaveChangesAsync(cancellationToken))
            .ReturnsAsync(1);

        // Act
        var result = await _service.SendSmsAsync(request, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<SmsNotificationModel>(), cancellationToken), Times.Once);
        _repositoryMock.Verify(r => r.UnitOfWork.SaveChangesAsync(cancellationToken), Times.Once);
        _senderMock.Verify(s => s.SendAsync(request.SenderId, request.RecipientId, It.IsAny<string>(), request.Body), Times.Once);
    }

    [Fact]
    public async Task SendMailAsync_ShouldHandleSenderException()
    {
        // Arrange
        var request = new SendSmsNotificationRequest(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            "+7912345678",
            "test message"
        );
        var cancellationToken = CancellationToken.None;

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<SmsNotificationModel>(), cancellationToken))
            .Returns(() => new ValueTask<SmsNotificationModel>());

        _senderMock.Setup(s => s.SendAsync(request.SenderId, request.RecipientId, It.IsAny<string>(), request.Body))
            .ThrowsAsync(new Exception("Test exception"));

        _repositoryMock.Setup(r => r.UnitOfWork.SaveChangesAsync(cancellationToken))
            .ReturnsAsync(1);

        // Act
        var result = await _service.SendSmsAsync(request, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Message == "Test exception");
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<SmsNotificationModel>(), cancellationToken), Times.Once);
        _repositoryMock.Verify(r => r.UnitOfWork.SaveChangesAsync(cancellationToken), Times.Once);
        _senderMock.Verify(s => s.SendAsync(request.SenderId, request.RecipientId, It.IsAny<string>(), request.Body), Times.Exactly(4));
    }

    [Fact]
    public async Task SendMailAsync_ShouldRetryOnFailure()
    {
        // Arrange
        var request = new SendSmsNotificationRequest(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            "+7912345678",
            "test message"
        );
        var cancellationToken = CancellationToken.None;

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<SmsNotificationModel>(), cancellationToken))
            .Returns(() => new ValueTask<SmsNotificationModel>());

        _senderMock.SetupSequence(s => s.SendAsync(request.SenderId, request.RecipientId, It.IsAny<string>(), request.Body))
            .ThrowsAsync(new Exception("First attempt failed"))
            .ThrowsAsync(new Exception("Second attempt failed"))
            .Returns(() => Task.CompletedTask);

        _repositoryMock.Setup(r => r.UnitOfWork.SaveChangesAsync(cancellationToken))
            .ReturnsAsync(1);

        // Act
        var result = await _service.SendSmsAsync(request, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<SmsNotificationModel>(), cancellationToken), Times.Once);
        _repositoryMock.Verify(r => r.UnitOfWork.SaveChangesAsync(cancellationToken), Times.Once);
        _senderMock.Verify(s => s.SendAsync(request.SenderId, request.RecipientId, It.IsAny<string>(), request.Body), Times.Exactly(3));
    }
}