using Core.Connections.UserIdDecorator.Interfaces;
using Core.Domain.Extensions;
using Microsoft.AspNetCore.Http;

namespace Core.Connections.UserIdDecorator;

public class HttpUserIdDecorator(IHttpContextAccessor contextAccessor) : IUserIdDecorator
{
    public Guid? GetUserId() => contextAccessor.HttpContext?.TryGetUserId();
}