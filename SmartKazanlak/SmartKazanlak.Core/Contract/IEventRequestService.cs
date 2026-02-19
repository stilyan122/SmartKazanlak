
using SmartKazanlak.Core.Models.EventRequest;
using SmartKazanlak.Models.EventRequest;

namespace SmartKazanlak.Core.Contract
{
    public interface IEventRequestService
    {
        Task<Guid> CreateAsync(CreateEventRequestDto dto, string userId, string userEmail, string organizer);
        Task<IReadOnlyList<EventRequestListDto>> GetMyAsync(string userId);

        Task<IReadOnlyList<EventRequestListDto>> GetPendingAsync();
        Task<IReadOnlyList<EventRequestListDto>> GetApprovedAsync();
        Task<IReadOnlyList<EventRequestListDto>> GetRejectedAsync();
        Task<EventRequestDetailsDto?> GetDetailsAsync(Guid id);

        Task MarkViewedAsync(Guid id, string adminUserId);
        Task ApproveAsync(Guid id, string adminUserId, string? note = null);
        Task RejectAsync(Guid id, string adminUserId, string note);
    }
}
