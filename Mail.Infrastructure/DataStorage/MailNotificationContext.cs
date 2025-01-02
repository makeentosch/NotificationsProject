using Core.Domain.Repositories.Interfaces;
using Mail.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Mail.Infrastructure.DataStorage;

public sealed class MailNotificationContext : DbContext, IUnitOfWork
{
    public DbSet<MailNotificationModel> Statuses { get; set; }

    public MailNotificationContext(DbContextOptions<MailNotificationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}