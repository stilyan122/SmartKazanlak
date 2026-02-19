using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartKazanlak.Core.Contract;
using SmartKazanlak.Core.Domain.Entities;

namespace SmartKazanlak.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }

        public DbSet<EventRequest> EventRequests => Set<EventRequest>();
        public DbSet<MediaFile> MediaFiles => Set<MediaFile>();
        public DbSet<ModerationAction> ModerationActions => Set<ModerationAction>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<EventRequest>(e =>
            {
                e.Property(x => x.Title).HasMaxLength(120).IsRequired();
                e.Property(x => x.Location).HasMaxLength(200).IsRequired();
                e.Property(x => x.Description).HasMaxLength(4000).IsRequired();

                e.Property(x => x.OrganizerName).HasMaxLength(120).IsRequired();
                e.Property(x => x.OrganizerEmail).HasMaxLength(256).IsRequired();
                e.Property(x => x.Phone).HasMaxLength(30).IsRequired();

                e.Property(x => x.AdminNote).HasMaxLength(1000);

                e.Property(x => x.Status)
                 .HasConversion<int>()  
                 .IsRequired();

                e.HasMany(x => x.MediaFiles)
                 .WithOne(x => x.EventRequest)
                 .HasForeignKey(x => x.EventRequestId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(x => x.ModerationActions)
                 .WithOne(x => x.EventRequest)
                 .HasForeignKey(x => x.EventRequestId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<MediaFile>(m =>
            {
                m.Property(x => x.Type)
                    .HasConversion<int>() 
                    .IsRequired();

                m.Property(x => x.OriginalFileName).HasMaxLength(255).IsRequired();
                m.Property(x => x.StoredFileName).HasMaxLength(255).IsRequired();
                m.Property(x => x.ContentType).HasMaxLength(100).IsRequired();
            });

            builder.Entity<ModerationAction>(a =>
            {
                a.Property(x => x.Action).HasMaxLength(30).IsRequired();
                a.Property(x => x.Note).HasMaxLength(1000);
            });
        }
    }
}
