using Microsoft.AspNetCore.Mvc;
using Push.Application.Services.Interfaces;

namespace Push.Api.Controllers;

[ApiController]
[Route("notifications/mail/status")]
public class StatusController(IStatusService statusService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var status = await statusService.GetStatusByIdAsync(id, cancellationToken);
        if (status.IsFailed)
            return NoContent();
        
        return Ok(status.Value);
    }
}