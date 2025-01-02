using Microsoft.AspNetCore.Mvc;
using Sms.Application.Services.Interfaces;

namespace Sms.Api.Controllers;

[ApiController]
[Route("notifications/mail/status")]
public class StatusController(IStatusService statusService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var status = await statusService.GetStatusByIdAsync(id, cancellationToken);
        if (status.IsFailed)
            return BadRequest(status.Errors);
        
        return Ok(status.Value);
    }
}