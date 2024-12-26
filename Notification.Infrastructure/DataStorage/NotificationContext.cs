using Core.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Notification.Domain.Models;

namespace Notification.Infrastructure.DataStorage;

public sealed class NotificationContext : DbContext, IUnitOfWork
{
    public DbSet<NotificationModel> Notifications { get; set; }

    public NotificationContext(DbContextOptions<NotificationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<NotificationModel>(builder =>
        {
            builder.OwnsMany(n => n.Attachments, attachment =>
            {
                attachment.Property(a => a.FileName).HasColumnName(nameof(Attachment.FileName));
                attachment.Property(a => a.FileUri).HasColumnName(nameof(Attachment.FileUri));
                attachment.Property(a => a.ContentType).HasColumnName(nameof(Attachment.ContentType));
            });
        });
    }
}