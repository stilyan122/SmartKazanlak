using Microsoft.EntityFrameworkCore;
using SmartKazanlak.Core.Domain.Entities;

namespace SmartKazanlak.Core.Contract
{
    public interface IApplicationDbContext
    {
        DbSet<EventRequest> EventRequests { get; }
        DbSet<MediaFile> MediaFiles { get; }
        DbSet<ModerationAction> ModerationActions { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
