using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Push.Application.Services;
using Push.Application.Services.Interfaces;
using Push.Domain.Interfaces;
using Push.Domain.Models;
using RabbitMqContracts.Push.Requests;

namespace Push.Test;

public class PushNotificationServiceTests
{
    private readonly Mock<IPushNotificationRepository> _repositoryMock;
    private readonly Mock<IPushNotificationSender> _senderMock;
    private readonly PushNotificationService _service;
    
    private readonly Dictionary<string, string> _inMemoryConfiguration = new()
    {
        { "Polly:DefaultAttempts", "3" },
        { "Polly:DefaultWaitTimeInSeconds", "5" }
    };

    public PushNotificationServiceTests()
    {
        _repositoryMock = new Mock<IPushNotificationRepository>();
        _senderMock = new Mock<IPushNotificationSender>();
        
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(_inMemoryConfiguration!)
            .Build();
        _service = new PushNotificationService(_repositoryMock.Object, _senderMock.Object, config);
    }

    [Fact]
    public async Task SendPushAsync_ShouldSendNotificationSuccessfully()
    {
        var request = new SendPushNotificationRequest(Guid.NewGuid(), "test title", "test_device_token", "test body");
        var cancellationToken = CancellationToken.None;

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<PushNotificationModel>(), cancellationToken))
            .Returns(() => new ValueTask<PushNotificationModel>());

        _senderMock.Setup(s => s.SendAsync(request.DeviceToken, request.Title, request.Body))
            .Returns(() => Task.CompletedTask);

        _repositoryMock.Setup(r => r.UnitOfWork.SaveChangesAsync(cancellationToken))
            .ReturnsAsync(1);

        
        var result = await _service.SendPushAsync(request, cancellationToken);

        
        result.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<PushNotificationModel>(), cancellationToken), Times.Once);
        _repositoryMock.Verify(r => r.UnitOfWork.SaveChangesAsync(cancellationToken), Times.Once);
        _senderMock.Verify(s => s.SendAsync(request.DeviceToken, request.Title, request.Body), Times.Once);
    }

    [Fact]
    public async Task SendPushAsync_ShouldHandleSenderException()
    {
        var request = new SendPushNotificationRequest(Guid.NewGuid(), "test title", "test_device_token", "test body");
        var cancellationToken = CancellationToken.None;

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<PushNotificationModel>(), cancellationToken))
            .Returns(() => new ValueTask<PushNotificationModel>());

        _senderMock.Setup(s => s.SendAsync(request.DeviceToken, request.Title, request.Body))
            .ThrowsAsync(new Exception("Test exception"));

        _repositoryMock.Setup(r => r.UnitOfWork.SaveChangesAsync(cancellationToken))
            .ReturnsAsync(1);

        
        var result = await _service.SendPushAsync(request, cancellationToken);

        
        result.IsSuccess.Should().BeFalse();
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<PushNotificationModel>(), cancellationToken), Times.Once);
        _repositoryMock.Verify(r => r.UnitOfWork.SaveChangesAsync(cancellationToken), Times.Once);
        _senderMock.Verify(s => s.SendAsync(request.DeviceToken, request.Title, request.Body), Times.Exactly(4));
    }

    [Fact]
    public async Task SendPushAsync_ShouldRetryOnFailure()
    {
        var request = new SendPushNotificationRequest(Guid.NewGuid(), "test title", "test_device_token", "test body");
        var cancellationToken = CancellationToken.None;

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<PushNotificationModel>(), cancellationToken))
            .Returns(() => new ValueTask<PushNotificationModel>());

        _senderMock.SetupSequence(s => s.SendAsync(request.DeviceToken, request.Title, request.Body))
            .ThrowsAsync(new Exception("First attempt failed"))
            .ThrowsAsync(new Exception("Second attempt failed"))
            .Returns(() => Task.CompletedTask);

        _repositoryMock.Setup(r => r.UnitOfWork.SaveChangesAsync(cancellationToken))
            .ReturnsAsync(1);
        

        var result = await _service.SendPushAsync(request, cancellationToken);
        

        result.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<PushNotificationModel>(), cancellationToken), Times.Once);
        _repositoryMock.Verify(r => r.UnitOfWork.SaveChangesAsync(cancellationToken), Times.Once);
        _senderMock.Verify(s => s.SendAsync(request.DeviceToken, request.Title, request.Body), Times.Exactly(3));
    }
}