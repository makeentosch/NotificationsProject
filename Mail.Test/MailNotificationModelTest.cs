using FluentAssertions;
using Mail.Domain.Models;

namespace Mail.Test;

public class MailNotificationModelTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dateOfCreate = DateTime.UtcNow;

        // Act
        var model = new MailNotificationModel(id, dateOfCreate);

        // Assert
        model.Id.Should().Be(id);
        model.DateOfCreate.Should().Be(dateOfCreate);
        model.Status.Should().Be(default); // Status не установлен, должно быть значение по умолчанию
    }

    [Fact]
    public async Task SetStatus_ShouldUpdateStatusAndDateOfUpdate()
    {
        // Arrange
        var model = new MailNotificationModel(Guid.NewGuid(), DateTime.UtcNow);
        var newStatus = NotificationStatus.Send;
        var initialUpdateDate = model.DateOfUpdate;
        await Task.Delay(100);

        // Act
        model.SetStatus(newStatus);

        // Assert
        model.Status.Should().Be(newStatus);
        model.DateOfUpdate.Should().BeAfter(initialUpdateDate);
    }

    [Fact]
    public void SetStatus_ShouldThrow_WhenTransitionFromSendToFail()
    {
        // Arrange
        var model = new MailNotificationModel(Guid.NewGuid(), DateTime.UtcNow);
        model.SetStatus(NotificationStatus.Send);

        // Act
        var act = () => model.SetStatus(NotificationStatus.Fail);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot set status to Send or Fail");
    }

    [Fact]
    public void SetDateOfUpdate_ShouldUpdateDateOfUpdate()
    {
        // Arrange
        var model = new MailNotificationModel(Guid.NewGuid(), DateTime.UtcNow);
        var newUpdateDate = DateTime.UtcNow.AddMinutes(10);

        // Act
        model.SetDateOfUpdate(newUpdateDate);

        // Assert
        model.DateOfUpdate.Should().Be(newUpdateDate);
    }
}