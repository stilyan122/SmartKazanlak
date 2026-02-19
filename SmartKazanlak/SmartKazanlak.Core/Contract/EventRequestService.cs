using Microsoft.EntityFrameworkCore;
using SmartKazanlak.Core.Domain.Entities;
using SmartKazanlak.Core.Enums;
using SmartKazanlak.Core.Models.EventRequest;
using SmartKazanlak.Models.EventRequest;

namespace SmartKazanlak.Core.Contract
{
    public class EventRequestService : IEventRequestService
    {
        private readonly IApplicationDbContext db;

        public EventRequestService(IApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<Guid> CreateAsync(CreateEventRequestDto dto, string userId, string userEmail, string organizer)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("userId is required.", nameof(userId));
            if (string.IsNullOrWhiteSpace(userEmail)) throw new ArgumentException("userEmail is required.", nameof(userEmail));

            if (dto.StartDateTime <= DateTime.Now)
                throw new InvalidOperationException("Event date/time must be in the future.");

            var request = new EventRequest
            {
                Title = dto.Title.Trim(),
                StartDateTime = dto.StartDateTime,
                Location = dto.Location.Trim(),
                Description = dto.Description.Trim(),
                OrganizerName = organizer,
                OrganizerEmail = userEmail.Trim(),
                Phone = dto.Phone.Trim(),

                Status = EventStatus.Pending,
                UserId = userId,

                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            db.EventRequests.Add(request);
            await db.SaveChangesAsync();

            return request.Id;
        }

        public async Task<IReadOnlyList<EventRequestListDto>> GetMyAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return Array.Empty<EventRequestListDto>();

            return await db.EventRequests
                .AsNoTracking()
                .Where(x => x.UserId == userId && x.Status != EventStatus.Deleted)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new EventRequestListDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    StartDateTime = x.StartDateTime,
                    Location = x.Location,
                    Status = x.Status.ToString(),
                    CreatedAt = x.CreatedAt,
                    AdminNote = x.AdminNote,
                    OrganizerName = x.User.FullName ?? "-"
                })
                .ToListAsync();
        }

        public Task<IReadOnlyList<EventRequestListDto>> GetPendingAsync()
            => GetByStatusAsync(EventStatus.Pending);

        public Task<IReadOnlyList<EventRequestListDto>> GetApprovedAsync()
            => GetByStatusAsync(EventStatus.Approved);

        public Task<IReadOnlyList<EventRequestListDto>> GetRejectedAsync()
            => GetByStatusAsync(EventStatus.Rejected);

        public async Task MarkViewedAsync(Guid id, string adminUserId)
        {
            if (id == Guid.Empty) throw new ArgumentException("Invalid id.", nameof(id));
            if (string.IsNullOrWhiteSpace(adminUserId)) throw new ArgumentException("adminUserId is required.", nameof(adminUserId));

            var exists = await db.ModerationActions
                .AsNoTracking()
                .AnyAsync(x => x.EventRequestId == id && x.AdminUserId == adminUserId && x.Action == "Viewed");

            if (exists) return;

            db.ModerationActions.Add(new ModerationAction
            {
                EventRequestId = id,
                AdminUserId = adminUserId,
                Action = "Viewed",
                ActionAt = DateTime.UtcNow
            });

            await db.SaveChangesAsync();
        }

        public async Task ApproveAsync(Guid id, string adminUserId, string? note = null)
        {
            if (id == Guid.Empty) throw new ArgumentException("Invalid id.", nameof(id));
            if (string.IsNullOrWhiteSpace(adminUserId)) throw new ArgumentException("adminUserId is required.", nameof(adminUserId));

            var req = await db.EventRequests.FirstOrDefaultAsync(x => x.Id == id);
            if (req is null) throw new InvalidOperationException("Event request not found.");

            if (req.Status != EventStatus.Pending)
                throw new InvalidOperationException("Only pending requests can be approved.");

            req.Status = EventStatus.Approved;
            req.AdminReviewedAt = DateTime.UtcNow;
            req.AdminReviewedByUserId = adminUserId;
            req.AdminNote = string.IsNullOrWhiteSpace(note) ? null : note.Trim();
            req.UpdatedAt = DateTime.UtcNow;

            db.ModerationActions.Add(new ModerationAction
            {
                EventRequestId = req.Id,
                AdminUserId = adminUserId,
                Action = "Approved",
                Note = req.AdminNote,
                ActionAt = DateTime.UtcNow
            });

            await db.SaveChangesAsync();
        }

        public async Task RejectAsync(Guid id, string adminUserId, string note)
        {
            if (id == Guid.Empty) throw new ArgumentException("Invalid id.", nameof(id));
            if (string.IsNullOrWhiteSpace(adminUserId)) throw new ArgumentException("adminUserId is required.", nameof(adminUserId));
            if (string.IsNullOrWhiteSpace(note)) throw new ArgumentException("Rejection note is required.", nameof(note));

            var req = await db.EventRequests.FirstOrDefaultAsync(x => x.Id == id);
            if (req is null) throw new InvalidOperationException("Event request not found.");

            if (req.Status != EventStatus.Pending)
                throw new InvalidOperationException("Only pending requests can be rejected.");

            req.Status = EventStatus.Rejected;
            req.AdminReviewedAt = DateTime.UtcNow;
            req.AdminReviewedByUserId = adminUserId;
            req.AdminNote = note.Trim();
            req.UpdatedAt = DateTime.UtcNow;

            db.ModerationActions.Add(new ModerationAction
            {
                EventRequestId = req.Id,
                AdminUserId = adminUserId,
                Action = "Rejected",
                Note = req.AdminNote,
                ActionAt = DateTime.UtcNow
            });

            await db.SaveChangesAsync();
        }

        public async Task<EventRequestDetailsDto?> GetDetailsAsync(Guid id)
        {
            return await db.EventRequests
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new EventRequestDetailsDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    StartDateTime = x.StartDateTime,
                    Location = x.Location,
                    Description = x.Description,
                    OrganizerName = x.OrganizerName,
                    OrganizerEmail = x.OrganizerEmail,
                    Phone = x.Phone,
                    Status = x.Status.ToString(),
                    CreatedAt = x.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        private async Task<IReadOnlyList<EventRequestListDto>> GetByStatusAsync(EventStatus status)
        {
            return await db.EventRequests
                .AsNoTracking()
                .Include(e => e.User)
                .Where(x => x.Status == status)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new EventRequestListDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    StartDateTime = x.StartDateTime,
                    Location = x.Location,
                    Status = x.Status.ToString(),
                    CreatedAt = x.CreatedAt,
                    OrganizerName = x.User.FullName ?? "-"
                })
                .ToListAsync();
        }
    }
}
