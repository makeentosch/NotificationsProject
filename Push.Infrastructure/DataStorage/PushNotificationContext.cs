using Core.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Push.Domain.Models;

namespace Push.Infrastructure.DataStorage;

public sealed class PushNotificationContext : DbContext, IUnitOfWork
{
    public DbSet<PushNotificationModel> Pushes { get; set; }

    public PushNotificationContext(DbContextOptions<PushNotificationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}