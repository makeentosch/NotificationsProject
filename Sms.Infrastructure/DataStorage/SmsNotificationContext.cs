using Core.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Sms.Domain.Models;

namespace Sms.Infrastructure.DataStorage;

public sealed class SmsNotificationContext : DbContext, IUnitOfWork
{
    public DbSet<SmsNotificationModel> Sms { get; set; }

    public SmsNotificationContext(DbContextOptions<SmsNotificationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}