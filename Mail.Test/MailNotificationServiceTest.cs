using FluentAssertions;
using Moq;
using Mail.Application.Services;
using Mail.Application.Services.Interfaces;
using Mail.Domain.Interfaces;
using Mail.Domain.Models;
using RabbitMqContracts.Mail.Requests;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using RabbitMqContracts.Mail.Dtos;

namespace Mail.Test;

public class MailNotificationServiceTest
{
    private readonly Mock<IMailNotificationRepository> _repositoryMock;
    private readonly Mock<IMailNotificationSender> _senderMock;
    private readonly MailNotificationService _service;
    
    private readonly Dictionary<string, string> _inMemoryConfiguration = new()
    {
        { "Polly:DefaultAttempts", "3" },
        { "Polly:DefaultWaitTimeInSeconds", "5" }
    };

    public MailNotificationServiceTest()
    {
        _repositoryMock = new Mock<IMailNotificationRepository>();
        _senderMock = new Mock<IMailNotificationSender>();
        
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(_inMemoryConfiguration!)
            .Build();
        _service = new MailNotificationService(_repositoryMock.Object, _senderMock.Object, config);
    }

    [Fact]
    public async Task SendMailAsync_ShouldSendNotificationSuccessfully()
    {
        // Arrange
        var request = new SendMailNotificationRequest(
            Guid.NewGuid(),
            "Test message",
            [],
            Guid.NewGuid(),
            "recipient@example.com",
            Guid.NewGuid()
        );
        var cancellationToken = CancellationToken.None;

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<MailNotificationModel>(), cancellationToken))
            .Returns(() => new ValueTask<MailNotificationModel>());

        _senderMock.Setup(s => s.SendAsync(request.SenderId, request.RecipientId, It.IsAny<MailAddress>(), request.Message, It.IsAny<IEnumerable<AttachmentDto>>()))
            .Returns(() => Task.CompletedTask);

        _repositoryMock.Setup(r => r.UnitOfWork.SaveChangesAsync(cancellationToken))
            .ReturnsAsync(1);

        // Act
        var result = await _service.SendMailAsync(request, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<MailNotificationModel>(), cancellationToken), Times.Once);
        _repositoryMock.Verify(r => r.UnitOfWork.SaveChangesAsync(cancellationToken), Times.Once);
        _senderMock.Verify(s => s.SendAsync(request.SenderId, request.RecipientId, It.IsAny<MailAddress>(), request.Message, It.IsAny<IEnumerable<AttachmentDto>>()), Times.Once);
    }

    [Fact]
    public async Task SendMailAsync_ShouldHandleSenderException()
    {
        // Arrange
        var request = new SendMailNotificationRequest(
            Guid.NewGuid(),
            "Test message",
            [],
            Guid.NewGuid(),
            "recipient@example.com",
            Guid.NewGuid()
        );
        var cancellationToken = CancellationToken.None;

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<MailNotificationModel>(), cancellationToken))
            .Returns(() => new ValueTask<MailNotificationModel>());

        _senderMock.Setup(s => s.SendAsync(request.SenderId, request.RecipientId, It.IsAny<MailAddress>(), request.Message, It.IsAny<IEnumerable<AttachmentDto>>()))
            .ThrowsAsync(new Exception("Test exception"));

        _repositoryMock.Setup(r => r.UnitOfWork.SaveChangesAsync(cancellationToken))
            .ReturnsAsync(1);

        // Act
        var result = await _service.SendMailAsync(request, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Message == "Test exception");
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<MailNotificationModel>(), cancellationToken), Times.Once);
        _repositoryMock.Verify(r => r.UnitOfWork.SaveChangesAsync(cancellationToken), Times.Once);
        _senderMock.Verify(s => s.SendAsync(request.SenderId, request.RecipientId, It.IsAny<MailAddress>(), request.Message, It.IsAny<IEnumerable<AttachmentDto>>()), Times.Exactly(4));
    }

    [Fact]
    public async Task SendMailAsync_ShouldRetryOnFailure()
    {
        // Arrange
        var request = new SendMailNotificationRequest(
            Guid.NewGuid(),
            "Test message",
            [],
            Guid.NewGuid(),
            "recipient@example.com",
            Guid.NewGuid()
        );
        var cancellationToken = CancellationToken.None;

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<MailNotificationModel>(), cancellationToken))
            .Returns(() => new ValueTask<MailNotificationModel>());

        _senderMock.SetupSequence(s => s.SendAsync(request.SenderId, request.RecipientId, It.IsAny<MailAddress>(), request.Message, It.IsAny<IEnumerable<AttachmentDto>>()))
            .ThrowsAsync(new Exception("First attempt failed"))
            .ThrowsAsync(new Exception("Second attempt failed"))
            .Returns(() => Task.CompletedTask);

        _repositoryMock.Setup(r => r.UnitOfWork.SaveChangesAsync(cancellationToken))
            .ReturnsAsync(1);

        // Act
        var result = await _service.SendMailAsync(request, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<MailNotificationModel>(), cancellationToken), Times.Once);
        _repositoryMock.Verify(r => r.UnitOfWork.SaveChangesAsync(cancellationToken), Times.Once);
        _senderMock.Verify(s => s.SendAsync(request.SenderId, request.RecipientId, It.IsAny<MailAddress>(), request.Message, It.IsAny<IEnumerable<AttachmentDto>>()), Times.Exactly(3));
    }
}