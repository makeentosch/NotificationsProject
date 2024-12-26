using Mapster;
using Notification.Api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Notification.Api.Controllers.Notifications.Requests;
using Notification.Api.Controllers.Notifications.Responses;
using Notification.Application.Interfaces;

namespace Notification.Api.Controllers.Notifications;

[ApiController]
[Route("api/v1/notifications")]
public class NotificationController(INotificationService notificationService)
    : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(200, Type = typeof(Guid))]
    public async Task<IActionResult> SendNotificationAsync([FromBody] NotificationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.Values);

        var notificationModel = request.ToNotificationModel(Guid.NewGuid(), DateTime.UtcNow);
        await notificationService.SendNotificationAsync(notificationModel);

        return Ok(notificationModel.Id);
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<NotificationResponse>))]
    [ProducesResponseType(204)]
    public async Task<IActionResult> GetNotificationsAsync()
    {
        var notifications = await notificationService.GetAllAsync();
        if (notifications.Count == 0)
            return NoContent();

        return Ok(notifications.Adapt<IEnumerable<NotificationResponse>>());
    }

    [HttpGet("{notificationId:guid}")]
    [ProducesResponseType(200, Type = typeof(NotificationResponse))]
    [ProducesResponseType(204)]
    public async Task<IActionResult> GetNotificationByIdAsync([FromRoute] Guid notificationId)
    {
        var notification = await notificationService.GetByIdAsync(notificationId);
        if (notification is null)
            return NoContent();

        return Ok(notification.Adapt<NotificationResponse>());
    }
    
    [HttpGet("{notificationId:guid}/status")]
    [ProducesResponseType(200, Type = typeof(NotificationWithStatusResponse))]
    [ProducesResponseType(204)]
    public async Task<IActionResult> GetNotificationByIdWithStatusAsync([FromRoute] Guid notificationId)
    {
        var notification = await notificationService.GetByIdAsync(notificationId);
        if (notification is null)
            return NoContent();

        var status = await notificationService.GetStatusesByIdAsync(notification.Id, notification.NotificationTypes);
        return Ok(notification.ToNotificationWithStatusResponse(status));
    }
}
